using AutoMapper;
using Contracts;
using Entities.DataTransfertObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace A_UN_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UniversitiesController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly string _baseURL;

        public UniversitiesController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _repository.Path = "/pictures/University";
            _baseURL = string.Concat(httpContextAccessor.HttpContext.Request.Scheme, "://", httpContextAccessor.HttpContext.Request.Host);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<UniversityReadDto>>> GetUniversities([FromQuery] UniversityQueryParameters universityQueryParameters)
        {
            //if (!universityQueryParameters.ValidBirthdayRange) return BadRequest("Max birthday cannot be less than min birthday");

            //if (!universityQueryParameters.ValidCreateAtRange) return BadRequest("Max create day cannot be less than min create day");

            var universities = await _repository.University.GetUniversitiesAsync(universityQueryParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(universities.MetaData));

            _logger.LogInfo($"Returned all universities from database.");

            var universitiesReadDto = _mapper.Map<IEnumerable<UniversityReadDto>>(universities);

            universitiesReadDto.ToList().ForEach(universityReadDto =>
            {
                if (!string.IsNullOrWhiteSpace(universityReadDto.ImgLink)) universityReadDto.ImgLink = $"{_baseURL}{universityReadDto.ImgLink}";
            });

            return Ok(universitiesReadDto);
        }



        [HttpGet("{id}", Name = "UniversityById")]
        public async Task<ActionResult<UniversityReadDto>> GetUniversityById(Guid id)
        {
            var university = await _repository.University.GetUniversityByIdAsync(id);

            if (university == null)
            {
                _logger.LogError($"University with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned universityWriteDto with id: {id}");

                var universityReadDto = _mapper.Map<UniversityReadDto>(university);

                if (!string.IsNullOrWhiteSpace(universityReadDto.ImgLink)) universityReadDto.ImgLink = $"{_baseURL}{universityReadDto.ImgLink}";

                return Ok(universityReadDto);
            }
        }



        [HttpPost]
        public async Task<ActionResult<UniversityReadDto>> CreateUniversity([FromBody] UniversityWriteDto university)
        {
            if (university == null)
            {
                _logger.LogError("University object sent from university is null.");
                return BadRequest("University object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid universityWriteDto object sent from university.");
                return BadRequest("Invalid model object");
            }

            //If the AppUserId is not provided, then affect the currenct logged In User Id
            if (string.IsNullOrWhiteSpace(university.AppUserId)) university.AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var universityEntity = _mapper.Map<University>(university);

            if (await _repository.University.UniversityExistAsync(universityEntity))
            {
                ModelState.AddModelError("", "This University exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.University.CreateUniversityAsync(universityEntity);
            await _repository.SaveAsync();

            var universityReadDto = _mapper.Map<UniversityReadDto>(universityEntity);

            if (!string.IsNullOrWhiteSpace(universityReadDto.ImgLink)) universityReadDto.ImgLink = $"{_baseURL}{universityReadDto.ImgLink}";
            
            return CreatedAtRoute("UniversityById", new { id = universityReadDto.Id }, universityReadDto);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<UniversityReadDto>> UpdateUniversity(Guid id, [FromBody] UniversityWriteDto universityWriteDto)
        {
            if (universityWriteDto == null)
            {
                _logger.LogError("University object sent from university is null.");
                return BadRequest("University object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid universityWriteDto object sent from university.");
                return BadRequest("Invalid model object");
            }

            var universityEntity = await _repository.University.GetUniversityByIdAsync(id);
            if (universityEntity == null)
            {
                _logger.LogError($"University with id: {id}, hasn't been found.");
                return NotFound();
            }
            
            //If the AppUserId is not provided, then affect the currenct logged In User Id
            if (string.IsNullOrWhiteSpace(universityWriteDto.AppUserId)) universityWriteDto.AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _mapper.Map(universityWriteDto, universityEntity);

            await _repository.University.UpdateUniversityAsync(universityEntity);
            await _repository.SaveAsync();

            var universityReadDto = _mapper.Map<UniversityReadDto>(universityEntity);

            if (!string.IsNullOrWhiteSpace(universityReadDto.ImgLink)) universityReadDto.ImgLink = $"{_baseURL}{universityReadDto.ImgLink}";

            return Ok(universityReadDto);
        }




        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialUniversityUpdate(Guid Id, JsonPatchDocument<UniversityWriteDto> patchDoc)
        {
            var universityModelFromRepository = await _repository.University.GetUniversityByIdAsync(Id);
            if (universityModelFromRepository == null) return NotFound();

            var universityToPatch = _mapper.Map<UniversityWriteDto>(universityModelFromRepository);
            patchDoc.ApplyTo(universityToPatch, ModelState);

            if (!TryValidateModel(universityToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(universityToPatch, universityModelFromRepository);

            await _repository.University.UpdateUniversityAsync(universityModelFromRepository);
                             
            await _repository.SaveAsync();

            return NoContent();
        }




        [HttpPut("{id}/upload-picture")]
        public async Task<ActionResult<UniversityReadDto>> UploadPicture(Guid id, [FromForm] IFormFile file)
        {
            var universityEntity = await _repository.University.GetUniversityByIdAsync(id);
            if (universityEntity == null) return NotFound();

            if (file != null)
            {
                _repository.File.FilePath = id.ToString();

                var uploadResult = await _repository.File.UploadFile(file);

                if (uploadResult == null)
                {
                    ModelState.AddModelError("", "something went wrong when uploading the picture");
                    return ValidationProblem(ModelState);
                }
                else
                {
                    universityEntity.ImgLink = uploadResult;
                }
            }

            await _repository.University.UpdateUniversityAsync(universityEntity);

            await _repository.SaveAsync();

            var universityReadDto = _mapper.Map<UniversityReadDto>(universityEntity);

            if (!string.IsNullOrWhiteSpace(universityReadDto.ImgLink)) universityReadDto.ImgLink = $"{_baseURL}{universityReadDto.ImgLink}";

            return Ok(universityReadDto);
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUniversity(Guid id)
        {
            var university = await _repository.University.GetUniversityByIdAsync(id);

            if (university == null)
            {
                _logger.LogError($"University with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.University.DeleteUniversityAsync(university);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}

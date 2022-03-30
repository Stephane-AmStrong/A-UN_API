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
    public class PersonalFilesController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly string _baseURL;

        public PersonalFilesController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _repository.Path = "/PersonalFile";
            _baseURL = string.Concat(httpContextAccessor.HttpContext.Request.Scheme, "://", httpContextAccessor.HttpContext.Request.Host);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonalFileReadDto>>> GetPersonalFiles([FromQuery] PersonalFileQueryParameters personalFileQueryParameters)
        {
            var personalFiles = await _repository.PersonalFile.GetPersonalFilesAsync(personalFileQueryParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(personalFiles.MetaData));

            _logger.LogInfo($"Returned all personalFiles from database.");

            var personalFilesReadDto = _mapper.Map<IEnumerable<PersonalFileReadDto>>(personalFiles);

            personalFilesReadDto.ToList().ForEach(personalFileReadDto =>
            {
                if (!string.IsNullOrWhiteSpace(personalFileReadDto.Link)) personalFileReadDto.Link = $"{_baseURL}{personalFileReadDto.Link}";
            });

            return Ok(personalFilesReadDto);
        }



        [HttpGet("{id}", Name = "PersonalFileById")]
        public async Task<ActionResult<PersonalFileReadDto>> GetPersonalFileById(Guid id)
        {
            var personalFile = await _repository.PersonalFile.GetPersonalFileByIdAsync(id);

            if (personalFile == null)
            {
                _logger.LogError($"PersonalFile with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned personalFileWriteDto with id: {id}");

                var personalFileReadDto = _mapper.Map<PersonalFileReadDto>(personalFile);

                if (!string.IsNullOrWhiteSpace(personalFileReadDto.Link)) personalFileReadDto.Link = $"{_baseURL}{personalFileReadDto.Link}";

                return Ok(personalFileReadDto);
            }
        }



        [HttpPost]
        public async Task<ActionResult<PersonalFileReadDto>> CreatePersonalFile([FromBody] PersonalFileWriteDto personalFile)
        {
            if (personalFile == null)
            {
                _logger.LogError("PersonalFile object sent from personalFile is null.");
                return BadRequest("PersonalFile object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid personalFileWriteDto object sent from personalFile.");
                return BadRequest("Invalid model object");
            }

            //If the AppUserId is not provided, then affect the currenct logged In User Id
            if (string.IsNullOrWhiteSpace(personalFile.AppUserId)) personalFile.AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var personalFileEntity = _mapper.Map<PersonalFile>(personalFile);

            if (await _repository.PersonalFile.PersonalFileExistAsync(personalFileEntity))
            {
                ModelState.AddModelError("", "This PersonalFile exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.PersonalFile.CreatePersonalFileAsync(personalFileEntity);
            await _repository.SaveAsync();

            var personalFileReadDto = _mapper.Map<PersonalFileReadDto>(personalFileEntity);
            return CreatedAtRoute("PersonalFileById", new { id = personalFileReadDto.Id }, personalFileReadDto);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<PersonalFileReadDto>> UpdatePersonalFile(Guid id, [FromBody] PersonalFileWriteDto personalFileWriteDto)
        {
            if (personalFileWriteDto == null)
            {
                _logger.LogError("PersonalFile object sent from personalFile is null.");
                return BadRequest("PersonalFile object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid personalFileWriteDto object sent from personalFile.");
                return BadRequest("Invalid model object");
            }

            var personalFile = await _repository.PersonalFile.GetPersonalFileByIdAsync(id);
            if (personalFile == null)
            {
                _logger.LogError($"PersonalFile with id: {id}, hasn't been found.");
                return NotFound();
            }

            //If the AppUserId is not provided, then affect the currenct logged In User Id
            if (string.IsNullOrWhiteSpace(personalFileWriteDto.AppUserId)) personalFileWriteDto.AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _mapper.Map(personalFileWriteDto, personalFile);

            await _repository.PersonalFile.UpdatePersonalFileAsync(personalFile);
            await _repository.SaveAsync();

            var personalFileReadDto = _mapper.Map<PersonalFileReadDto>(personalFile);

            if (!string.IsNullOrWhiteSpace(personalFileReadDto.Link)) personalFileReadDto.Link = $"{_baseURL}{personalFileReadDto.Link}";

            return Ok(personalFileReadDto);
        }



        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialPersonalFileUpdate(Guid Id, JsonPatchDocument<PersonalFileWriteDto> patchDoc)
        {
            var personalFile = await _repository.PersonalFile.GetPersonalFileByIdAsync(Id);
            if (personalFile == null) return NotFound();

            var branchToPatch = _mapper.Map<PersonalFileWriteDto>(personalFile);
            patchDoc.ApplyTo(branchToPatch, ModelState);

            if (!TryValidateModel(branchToPatch))
            {
                return ValidationProblem(ModelState);
            }

            //If the AppUserId is not provided, then affect the currenct logged In User Id
            if (string.IsNullOrWhiteSpace(personalFile.AppUserId)) personalFile.AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _mapper.Map(branchToPatch, personalFile);

            await _repository.PersonalFile.UpdatePersonalFileAsync(personalFile);

            await _repository.SaveAsync();

            return NoContent();
        }



        [HttpPut("{id}/upload-file")]
        public async Task<ActionResult<PersonalFileReadDto>> UploadPicture(Guid id, [FromForm] IFormFile file)
        {
            var personalFileEntity = await _repository.PersonalFile.GetPersonalFileByIdAsync(id);
            if (personalFileEntity == null) return NotFound();

            if (file != null)
            {
                _repository.File.FilePath = id.ToString();

                var uploadResult = await _repository.File.UploadFile(file);

                if (uploadResult == null)
                {
                    ModelState.AddModelError("", "something went wrong when uploading the file");
                    return ValidationProblem(ModelState);
                }
                else
                {
                    personalFileEntity.Link = uploadResult;
                }
            }

            await _repository.PersonalFile.UpdatePersonalFileAsync(personalFileEntity);

            await _repository.SaveAsync();

            var personalFileReadDto = _mapper.Map<PersonalFileReadDto>(personalFileEntity);

            if (!string.IsNullOrWhiteSpace(personalFileReadDto.Link)) personalFileReadDto.Link = $"{_baseURL}{personalFileReadDto.Link}";

            return Ok(personalFileReadDto);
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePersonalFile(Guid id)
        {
            var personalFile = await _repository.PersonalFile.GetPersonalFileByIdAsync(id);

            if (personalFile == null)
            {
                _logger.LogError($"PersonalFile with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.PersonalFile.DeletePersonalFileAsync(personalFile);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}

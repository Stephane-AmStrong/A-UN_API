using AutoMapper;
using Contracts;
using Entities.DataTransfertObjects;
using Entities.Models;
using Entities.Models.QueryParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GesProdAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UniversitiesController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public UniversitiesController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<UniversityReadDto>>> GetAllUniversities([FromQuery] QueryStringParameters paginationParameters)
        {
            var Universities = await _repository.University.GetAllUniversitiesAsync(paginationParameters);

            var metadata = new
            {
                Universities.TotalCount,
                Universities.PageSize,
                Universities.CurrentPage,
                Universities.TotalPages,
                Universities.HasNext,
                Universities.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            _logger.LogInfo($"Returned all Universities from database.");

            var UniversitiesReadDto = _mapper.Map<IEnumerable<UniversityReadDto>>(Universities);

            return Ok(UniversitiesReadDto);
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

            var universityEntity = _mapper.Map<University>(university);

            if (await _repository.University.UniversityExistAsync(universityEntity))
            {
                ModelState.AddModelError("", "This University exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.University.CreateUniversityAsync(universityEntity);
            await _repository.SaveAsync();

            var universityReadDto = _mapper.Map<UniversityReadDto>(universityEntity);
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

            _mapper.Map(universityWriteDto, universityEntity);


            await _repository.University.UpdateUniversityAsync(universityEntity);
            await _repository.SaveAsync();

            var universityReadDto = _mapper.Map<UniversityReadDto>(universityEntity);
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

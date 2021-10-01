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
    public class AcademicYearsController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public AcademicYearsController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<AcademicYearReadDto>>> GetAllAcademicYears([FromQuery] QueryStringParameters paginationParameters)
        {
            var academicYears = await _repository.AcademicYear.GetAllAcademicYearsAsync(paginationParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(academicYears.MetaData));

            _logger.LogInfo($"Returned all academicYears from database.");

            var academicYearsReadDto = _mapper.Map<IEnumerable<AcademicYearReadDto>>(academicYears);

            return Ok(academicYearsReadDto);
        }



        [HttpGet("{id}", Name = "AcademicYearById")]
        public async Task<ActionResult<AcademicYearReadDto>> GetAcademicYearById(Guid id)
        {
            var academicYear = await _repository.AcademicYear.GetAcademicYearByIdAsync(id);

            if (academicYear == null)
            {
                _logger.LogError($"AcademicYear with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned academicYearWriteDto with id: {id}");

                var academicYearReadDto = _mapper.Map<AcademicYearReadDto>(academicYear);
                
                return Ok(academicYearReadDto);
            }
        }



        [HttpPost]
        public async Task<ActionResult<AcademicYearReadDto>> CreateAcademicYear([FromBody] AcademicYearWriteDto academicYear)
        {
            if (academicYear == null)
            {
                _logger.LogError("AcademicYear object sent from academicYear is null.");
                return BadRequest("AcademicYear object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid academicYearWriteDto object sent from academicYear.");
                return BadRequest("Invalid model object");
            }

            var academicYearEntity = _mapper.Map<AcademicYear>(academicYear);

            if (await _repository.AcademicYear.AcademicYearExistAsync(academicYearEntity))
            {
                ModelState.AddModelError("", "This AcademicYear exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.AcademicYear.CreateAcademicYearAsync(academicYearEntity);
            await _repository.SaveAsync();

            var academicYearReadDto = _mapper.Map<AcademicYearReadDto>(academicYearEntity);
            return CreatedAtRoute("AcademicYearById", new { id = academicYearReadDto.Id }, academicYearReadDto);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<AcademicYearReadDto>> UpdateAcademicYear(Guid id, [FromBody] AcademicYearWriteDto academicYearWriteDto)
        {
            if (academicYearWriteDto == null)
            {
                _logger.LogError("AcademicYear object sent from academicYear is null.");
                return BadRequest("AcademicYear object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid academicYearWriteDto object sent from academicYear.");
                return BadRequest("Invalid model object");
            }

            var academicYearEntity = await _repository.AcademicYear.GetAcademicYearByIdAsync(id);
            if (academicYearEntity == null)
            {
                _logger.LogError($"AcademicYear with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(academicYearWriteDto, academicYearEntity);


            await _repository.AcademicYear.UpdateAcademicYearAsync(academicYearEntity);
            await _repository.SaveAsync();

            var academicYearReadDto = _mapper.Map<AcademicYearReadDto>(academicYearEntity);
            return Ok(academicYearReadDto);
        }




        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialAcademicYearUpdate(Guid Id, JsonPatchDocument<AcademicYearWriteDto> patchDoc)
        {
            var academicYearModelFromRepository = await _repository.AcademicYear.GetAcademicYearByIdAsync(Id);
            if (academicYearModelFromRepository == null) return NotFound();

            var academicYearToPatch = _mapper.Map<AcademicYearWriteDto>(academicYearModelFromRepository);
            patchDoc.ApplyTo(academicYearToPatch, ModelState);

            if (!TryValidateModel(academicYearToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(academicYearToPatch, academicYearModelFromRepository);

            await _repository.AcademicYear.UpdateAcademicYearAsync(academicYearModelFromRepository);
                             
            await _repository.SaveAsync();

            return NoContent();
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAcademicYear(Guid id)
        {
            var academicYear = await _repository.AcademicYear.GetAcademicYearByIdAsync(id);

            if (academicYear == null)
            {
                _logger.LogError($"AcademicYear with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.AcademicYear.DeleteAcademicYearAsync(academicYear);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}

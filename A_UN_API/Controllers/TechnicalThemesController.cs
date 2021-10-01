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
    public class TechnicalThemesController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public TechnicalThemesController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TechnicalThemeReadDto>>> GetAllTechnicalThemes([FromQuery] QueryStringParameters paginationParameters)
        {
            var technicalThemes = await _repository.TechnicalTheme.GetAllTechnicalThemesAsync(paginationParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(technicalThemes.MetaData));

            _logger.LogInfo($"Returned all technicalThemes from database.");

            var technicalThemesReadDto = _mapper.Map<IEnumerable<TechnicalThemeReadDto>>(technicalThemes);

            return Ok(technicalThemesReadDto);
        }



        [HttpGet("{id}", Name = "TechnicalThemeById")]
        public async Task<ActionResult<TechnicalThemeReadDto>> GetTechnicalThemeById(Guid id)
        {
            var technicalTheme = await _repository.TechnicalTheme.GetTechnicalThemeByIdAsync(id);

            if (technicalTheme == null)
            {
                _logger.LogError($"TechnicalTheme with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned technicalThemeWriteDto with id: {id}");

                var technicalThemeReadDto = _mapper.Map<TechnicalThemeReadDto>(technicalTheme);
                
                return Ok(technicalThemeReadDto);
            }
        }



        [HttpPost]
        public async Task<ActionResult<TechnicalThemeReadDto>> CreateTechnicalTheme([FromBody] TechnicalThemeWriteDto technicalTheme)
        {
            if (technicalTheme == null)
            {
                _logger.LogError("TechnicalTheme object sent from technicalTheme is null.");
                return BadRequest("TechnicalTheme object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid technicalThemeWriteDto object sent from technicalTheme.");
                return BadRequest("Invalid model object");
            }

            var technicalThemeEntity = _mapper.Map<TechnicalTheme>(technicalTheme);

            if (await _repository.TechnicalTheme.TechnicalThemeExistAsync(technicalThemeEntity))
            {
                ModelState.AddModelError("", "This TechnicalTheme exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.TechnicalTheme.CreateTechnicalThemeAsync(technicalThemeEntity);
            await _repository.SaveAsync();

            var technicalThemeReadDto = _mapper.Map<TechnicalThemeReadDto>(technicalThemeEntity);
            return CreatedAtRoute("TechnicalThemeById", new { id = technicalThemeReadDto.Id }, technicalThemeReadDto);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<TechnicalThemeReadDto>> UpdateTechnicalTheme(Guid id, [FromBody] TechnicalThemeWriteDto technicalThemeWriteDto)
        {
            if (technicalThemeWriteDto == null)
            {
                _logger.LogError("TechnicalTheme object sent from technicalTheme is null.");
                return BadRequest("TechnicalTheme object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid technicalThemeWriteDto object sent from technicalTheme.");
                return BadRequest("Invalid model object");
            }

            var technicalThemeEntity = await _repository.TechnicalTheme.GetTechnicalThemeByIdAsync(id);
            if (technicalThemeEntity == null)
            {
                _logger.LogError($"TechnicalTheme with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(technicalThemeWriteDto, technicalThemeEntity);


            await _repository.TechnicalTheme.UpdateTechnicalThemeAsync(technicalThemeEntity);
            await _repository.SaveAsync();

            var technicalThemeReadDto = _mapper.Map<TechnicalThemeReadDto>(technicalThemeEntity);
            return Ok(technicalThemeReadDto);
        }


        // PATCH api/technicalThemes/{id}
        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialTechnicalThemeUpdate(Guid Id, JsonPatchDocument<TechnicalThemeWriteDto> patchDoc)
        {
            var technicalThemeModelFromRepository = await _repository.TechnicalTheme.GetTechnicalThemeByIdAsync(Id);
            if (technicalThemeModelFromRepository == null) return NotFound();

            var technicalThemeToPatch = _mapper.Map<TechnicalThemeWriteDto>(technicalThemeModelFromRepository);
            patchDoc.ApplyTo(technicalThemeToPatch, ModelState);

            if (!TryValidateModel(technicalThemeToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(technicalThemeToPatch, technicalThemeModelFromRepository);

            await _repository.TechnicalTheme.UpdateTechnicalThemeAsync(technicalThemeModelFromRepository);
                             
            await _repository.SaveAsync();

            return NoContent();
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTechnicalTheme(Guid id)
        {
            var technicalTheme = await _repository.TechnicalTheme.GetTechnicalThemeByIdAsync(id);

            if (technicalTheme == null)
            {
                _logger.LogError($"TechnicalTheme with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.TechnicalTheme.DeleteTechnicalThemeAsync(technicalTheme);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}

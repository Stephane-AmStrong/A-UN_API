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
using System.Threading.Tasks;

namespace A_UN_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PrerequisitesController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public PrerequisitesController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrerequisiteReadDto>>> GetPrerequisites([FromQuery] PrerequisiteQueryParameters prerequisiteQueryParameters)
        {
            var registrationForms = await _repository.Prerequisite.GetPrerequisitesAsync(prerequisiteQueryParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(registrationForms.MetaData));

            _logger.LogInfo($"Returned all registrationForms from database.");

            var registrationFormsReadDto = _mapper.Map<IEnumerable<PrerequisiteReadDto>>(registrationForms);

            return Ok(registrationFormsReadDto);
        }



        [HttpGet("{id}", Name = "PrerequisiteById")]
        public async Task<ActionResult<PrerequisiteReadDto>> GetPrerequisiteById(Guid id)
        {
            var registrationForm = await _repository.Prerequisite.GetPrerequisiteByIdAsync(id);

            if (registrationForm == null)
            {
                _logger.LogError($"Prerequisite with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned registrationFormWriteDto with id: {id}");

                var registrationFormReadDto = _mapper.Map<PrerequisiteReadDto>(registrationForm);
                
                return Ok(registrationFormReadDto);
            }
        }



        [HttpPost]
        public async Task<ActionResult<PrerequisiteReadDto>> CreatePrerequisite([FromBody] PrerequisiteWriteDto registrationForm)
        {
            if (registrationForm == null)
            {
                _logger.LogError("Prerequisite object sent from registrationForm is null.");
                return BadRequest("Prerequisite object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid registrationFormWriteDto object sent from registrationForm.");
                return BadRequest("Invalid model object");
            }

            var registrationFormEntity = _mapper.Map<Prerequisite>(registrationForm);

            if (await _repository.Prerequisite.PrerequisiteExistAsync(registrationFormEntity))
            {
                ModelState.AddModelError("", "This Prerequisite exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.Prerequisite.CreatePrerequisiteAsync(registrationFormEntity);
            await _repository.SaveAsync();

            var registrationFormReadDto = _mapper.Map<PrerequisiteReadDto>(registrationFormEntity);
            return CreatedAtRoute("PrerequisiteById", new { id = registrationFormReadDto.Id }, registrationFormReadDto);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<PrerequisiteReadDto>> UpdatePrerequisite(Guid id, [FromBody] PrerequisiteWriteDto registrationFormWriteDto)
        {
            if (registrationFormWriteDto == null)
            {
                _logger.LogError("Prerequisite object sent from registrationForm is null.");
                return BadRequest("Prerequisite object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid registrationFormWriteDto object sent from registrationForm.");
                return BadRequest("Invalid model object");
            }

            var registrationFormEntity = await _repository.Prerequisite.GetPrerequisiteByIdAsync(id);
            if (registrationFormEntity == null)
            {
                _logger.LogError($"Prerequisite with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(registrationFormWriteDto, registrationFormEntity);


            await _repository.Prerequisite.UpdatePrerequisiteAsync(registrationFormEntity);
            await _repository.SaveAsync();

            var registrationFormReadDto = _mapper.Map<PrerequisiteReadDto>(registrationFormEntity);
            return Ok(registrationFormReadDto);
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePrerequisite(Guid id)
        {
            var registrationForm = await _repository.Prerequisite.GetPrerequisiteByIdAsync(id);

            if (registrationForm == null)
            {
                _logger.LogError($"Prerequisite with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.Prerequisite.DeletePrerequisiteAsync(registrationForm);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}

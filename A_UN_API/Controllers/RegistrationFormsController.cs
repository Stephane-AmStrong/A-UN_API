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
    public class RegistrationFormsController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public RegistrationFormsController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegistrationFormReadDto>>> GetAllRegistrationForms([FromQuery] QueryStringParameters paginationParameters)
        {
            var registrationForms = await _repository.RegistrationForm.GetAllRegistrationFormsAsync(paginationParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(registrationForms.MetaData));

            _logger.LogInfo($"Returned all registrationForms from database.");

            var registrationFormsReadDto = _mapper.Map<IEnumerable<RegistrationFormReadDto>>(registrationForms);

            return Ok(registrationFormsReadDto);
        }



        [HttpGet("{id}", Name = "RegistrationFormById")]
        public async Task<ActionResult<RegistrationFormReadDto>> GetRegistrationFormById(Guid id)
        {
            var registrationForm = await _repository.RegistrationForm.GetRegistrationFormByIdAsync(id);

            if (registrationForm == null)
            {
                _logger.LogError($"RegistrationForm with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned registrationFormWriteDto with id: {id}");

                var registrationFormReadDto = _mapper.Map<RegistrationFormReadDto>(registrationForm);
                
                return Ok(registrationFormReadDto);
            }
        }



        [HttpPost]
        public async Task<ActionResult<RegistrationFormReadDto>> CreateRegistrationForm([FromBody] RegistrationFormWriteDto registrationForm)
        {
            if (registrationForm == null)
            {
                _logger.LogError("RegistrationForm object sent from registrationForm is null.");
                return BadRequest("RegistrationForm object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid registrationFormWriteDto object sent from registrationForm.");
                return BadRequest("Invalid model object");
            }

            var registrationFormEntity = _mapper.Map<RegistrationForm>(registrationForm);

            if (await _repository.RegistrationForm.RegistrationFormExistAsync(registrationFormEntity))
            {
                ModelState.AddModelError("", "This RegistrationForm exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.RegistrationForm.CreateRegistrationFormAsync(registrationFormEntity);
            await _repository.SaveAsync();

            var registrationFormReadDto = _mapper.Map<RegistrationFormReadDto>(registrationFormEntity);
            return CreatedAtRoute("RegistrationFormById", new { id = registrationFormReadDto.Id }, registrationFormReadDto);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<RegistrationFormReadDto>> UpdateRegistrationForm(Guid id, [FromBody] RegistrationFormWriteDto registrationFormWriteDto)
        {
            if (registrationFormWriteDto == null)
            {
                _logger.LogError("RegistrationForm object sent from registrationForm is null.");
                return BadRequest("RegistrationForm object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid registrationFormWriteDto object sent from registrationForm.");
                return BadRequest("Invalid model object");
            }

            var registrationFormEntity = await _repository.RegistrationForm.GetRegistrationFormByIdAsync(id);
            if (registrationFormEntity == null)
            {
                _logger.LogError($"RegistrationForm with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(registrationFormWriteDto, registrationFormEntity);


            await _repository.RegistrationForm.UpdateRegistrationFormAsync(registrationFormEntity);
            await _repository.SaveAsync();

            var registrationFormReadDto = _mapper.Map<RegistrationFormReadDto>(registrationFormEntity);
            return Ok(registrationFormReadDto);
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRegistrationForm(Guid id)
        {
            var registrationForm = await _repository.RegistrationForm.GetRegistrationFormByIdAsync(id);

            if (registrationForm == null)
            {
                _logger.LogError($"RegistrationForm with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.RegistrationForm.DeleteRegistrationFormAsync(registrationForm);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}

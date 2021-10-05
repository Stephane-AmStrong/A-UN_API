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

namespace GesProdAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RegistrationFormLinesController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public RegistrationFormLinesController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegistrationFormLineReadDto>>> GetAllRegistrationFormLines([FromQuery] QueryStringParameters paginationParameters)
        {
            var registrationFormLines = await _repository.RegistrationFormLine.GetAllRegistrationFormLinesAsync(paginationParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(registrationFormLines.MetaData));

            _logger.LogInfo($"Returned all registrationFormLines from database.");

            var registrationFormLinesReadDto = _mapper.Map<IEnumerable<RegistrationFormLineReadDto>>(registrationFormLines);

            return Ok(registrationFormLinesReadDto);
        }



        [HttpGet("{id}", Name = "RegistrationFormLineById")]
        public async Task<ActionResult<RegistrationFormLineReadDto>> GetRegistrationFormLineById(Guid id)
        {
            var registrationFormLine = await _repository.RegistrationFormLine.GetRegistrationFormLineByIdAsync(id);

            if (registrationFormLine == null)
            {
                _logger.LogError($"RegistrationFormLine with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned registrationFormLineWriteDto with id: {id}");

                var registrationFormLineReadDto = _mapper.Map<RegistrationFormLineReadDto>(registrationFormLine);
                
                return Ok(registrationFormLineReadDto);
            }
        }



        [HttpPost]
        public async Task<ActionResult<RegistrationFormLineReadDto>> CreateRegistrationFormLine([FromBody] RegistrationFormLineWriteDto registrationFormLine)
        {
            if (registrationFormLine == null)
            {
                _logger.LogError("RegistrationFormLine object sent from registrationFormLine is null.");
                return BadRequest("RegistrationFormLine object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid registrationFormLineWriteDto object sent from registrationFormLine.");
                return BadRequest("Invalid model object");
            }

            var registrationFormLineEntity = _mapper.Map<RegistrationFormLine>(registrationFormLine);

            if (await _repository.RegistrationFormLine.RegistrationFormLineExistAsync(registrationFormLineEntity))
            {
                ModelState.AddModelError("", "This RegistrationFormLine exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.RegistrationFormLine.CreateRegistrationFormLineAsync(registrationFormLineEntity);
            await _repository.SaveAsync();

            var registrationFormLineReadDto = _mapper.Map<RegistrationFormLineReadDto>(registrationFormLineEntity);
            return CreatedAtRoute("RegistrationFormLineById", new { id = registrationFormLineReadDto.Id }, registrationFormLineReadDto);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<RegistrationFormLineReadDto>> UpdateRegistrationFormLine(Guid id, [FromBody] RegistrationFormLineWriteDto registrationFormLineWriteDto)
        {
            if (registrationFormLineWriteDto == null)
            {
                _logger.LogError("RegistrationFormLine object sent from registrationFormLine is null.");
                return BadRequest("RegistrationFormLine object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid registrationFormLineWriteDto object sent from registrationFormLine.");
                return BadRequest("Invalid model object");
            }

            var registrationFormLineEntity = await _repository.RegistrationFormLine.GetRegistrationFormLineByIdAsync(id);
            if (registrationFormLineEntity == null)
            {
                _logger.LogError($"RegistrationFormLine with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(registrationFormLineWriteDto, registrationFormLineEntity);


            await _repository.RegistrationFormLine.UpdateRegistrationFormLineAsync(registrationFormLineEntity);
            await _repository.SaveAsync();

            var registrationFormLineReadDto = _mapper.Map<RegistrationFormLineReadDto>(registrationFormLineEntity);
            return Ok(registrationFormLineReadDto);
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRegistrationFormLine(Guid id)
        {
            var registrationFormLine = await _repository.RegistrationFormLine.GetRegistrationFormLineByIdAsync(id);

            if (registrationFormLine == null)
            {
                _logger.LogError($"RegistrationFormLine with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.RegistrationFormLine.DeleteRegistrationFormLineAsync(registrationFormLine);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}

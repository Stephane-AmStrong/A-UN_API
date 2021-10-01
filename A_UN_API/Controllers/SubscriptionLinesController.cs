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
    public class SubscriptionLinesController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public SubscriptionLinesController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscriptionLineReadDto>>> GetAllSubscriptionLines([FromQuery] QueryStringParameters paginationParameters)
        {
            var subscriptionLines = await _repository.SubscriptionLine.GetAllSubscriptionLinesAsync(paginationParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(subscriptionLines.MetaData));

            _logger.LogInfo($"Returned all subscriptionLines from database.");

            var subscriptionLinesReadDto = _mapper.Map<IEnumerable<SubscriptionLineReadDto>>(subscriptionLines);

            return Ok(subscriptionLinesReadDto);
        }



        [HttpGet("{id}", Name = "SubscriptionLineById")]
        public async Task<ActionResult<SubscriptionLineReadDto>> GetSubscriptionLineById(Guid id)
        {
            var subscriptionLine = await _repository.SubscriptionLine.GetSubscriptionLineByIdAsync(id);

            if (subscriptionLine == null)
            {
                _logger.LogError($"SubscriptionLine with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned subscriptionLineWriteDto with id: {id}");

                var subscriptionLineReadDto = _mapper.Map<SubscriptionLineReadDto>(subscriptionLine);
                
                return Ok(subscriptionLineReadDto);
            }
        }



        [HttpPost]
        public async Task<ActionResult<SubscriptionLineReadDto>> CreateSubscriptionLine([FromBody] SubscriptionLineWriteDto subscriptionLine)
        {
            if (subscriptionLine == null)
            {
                _logger.LogError("SubscriptionLine object sent from subscriptionLine is null.");
                return BadRequest("SubscriptionLine object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid subscriptionLineWriteDto object sent from subscriptionLine.");
                return BadRequest("Invalid model object");
            }

            var subscriptionLineEntity = _mapper.Map<SubscriptionLine>(subscriptionLine);

            if (await _repository.SubscriptionLine.SubscriptionLineExistAsync(subscriptionLineEntity))
            {
                ModelState.AddModelError("", "This SubscriptionLine exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.SubscriptionLine.CreateSubscriptionLineAsync(subscriptionLineEntity);
            await _repository.SaveAsync();

            var subscriptionLineReadDto = _mapper.Map<SubscriptionLineReadDto>(subscriptionLineEntity);
            return CreatedAtRoute("SubscriptionLineById", new { id = subscriptionLineReadDto.Id }, subscriptionLineReadDto);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<SubscriptionLineReadDto>> UpdateSubscriptionLine(Guid id, [FromBody] SubscriptionLineWriteDto subscriptionLineWriteDto)
        {
            if (subscriptionLineWriteDto == null)
            {
                _logger.LogError("SubscriptionLine object sent from subscriptionLine is null.");
                return BadRequest("SubscriptionLine object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid subscriptionLineWriteDto object sent from subscriptionLine.");
                return BadRequest("Invalid model object");
            }

            var subscriptionLineEntity = await _repository.SubscriptionLine.GetSubscriptionLineByIdAsync(id);
            if (subscriptionLineEntity == null)
            {
                _logger.LogError($"SubscriptionLine with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(subscriptionLineWriteDto, subscriptionLineEntity);


            await _repository.SubscriptionLine.UpdateSubscriptionLineAsync(subscriptionLineEntity);
            await _repository.SaveAsync();

            var subscriptionLineReadDto = _mapper.Map<SubscriptionLineReadDto>(subscriptionLineEntity);
            return Ok(subscriptionLineReadDto);
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSubscriptionLine(Guid id)
        {
            var subscriptionLine = await _repository.SubscriptionLine.GetSubscriptionLineByIdAsync(id);

            if (subscriptionLine == null)
            {
                _logger.LogError($"SubscriptionLine with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.SubscriptionLine.DeleteSubscriptionLineAsync(subscriptionLine);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}

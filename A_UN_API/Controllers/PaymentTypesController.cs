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
    public class PaymentTypesController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public PaymentTypesController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentTypeReadDto>>> GetAllPaymentTypes([FromQuery] QueryStringParameters paginationParameters)
        {
            var paymentTypes = await _repository.PaymentType.GetAllPaymentTypesAsync(paginationParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paymentTypes.MetaData));

            _logger.LogInfo($"Returned all paymentTypes from database.");

            var paymentTypesReadDto = _mapper.Map<IEnumerable<PaymentTypeReadDto>>(paymentTypes);

            return Ok(paymentTypesReadDto);
        }



        [HttpGet("{id}", Name = "PaymentTypeById")]
        public async Task<ActionResult<PaymentTypeReadDto>> GetPaymentTypeById(Guid id)
        {
            var paymentType = await _repository.PaymentType.GetPaymentTypeByIdAsync(id);

            if (paymentType == null)
            {
                _logger.LogError($"PaymentType with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned paymentTypeWriteDto with id: {id}");

                var paymentTypeReadDto = _mapper.Map<PaymentTypeReadDto>(paymentType);
                
                return Ok(paymentTypeReadDto);
            }
        }



        [HttpPost]
        public async Task<ActionResult<PaymentTypeReadDto>> CreatePaymentType([FromBody] PaymentTypeWriteDto paymentType)
        {
            if (paymentType == null)
            {
                _logger.LogError("PaymentType object sent from paymentType is null.");
                return BadRequest("PaymentType object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid paymentTypeWriteDto object sent from paymentType.");
                return BadRequest("Invalid model object");
            }

            var paymentTypeEntity = _mapper.Map<PaymentType>(paymentType);

            if (await _repository.PaymentType.PaymentTypeExistAsync(paymentTypeEntity))
            {
                ModelState.AddModelError("", "This PaymentType exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.PaymentType.CreatePaymentTypeAsync(paymentTypeEntity);
            await _repository.SaveAsync();

            var paymentTypeReadDto = _mapper.Map<PaymentTypeReadDto>(paymentTypeEntity);
            return CreatedAtRoute("PaymentTypeById", new { id = paymentTypeReadDto.Id }, paymentTypeReadDto);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<PaymentTypeReadDto>> UpdatePaymentType(Guid id, [FromBody] PaymentTypeWriteDto paymentTypeWriteDto)
        {
            if (paymentTypeWriteDto == null)
            {
                _logger.LogError("PaymentType object sent from paymentType is null.");
                return BadRequest("PaymentType object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid paymentTypeWriteDto object sent from paymentType.");
                return BadRequest("Invalid model object");
            }

            var paymentTypeEntity = await _repository.PaymentType.GetPaymentTypeByIdAsync(id);
            if (paymentTypeEntity == null)
            {
                _logger.LogError($"PaymentType with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(paymentTypeWriteDto, paymentTypeEntity);


            await _repository.PaymentType.UpdatePaymentTypeAsync(paymentTypeEntity);
            await _repository.SaveAsync();

            var paymentTypeReadDto = _mapper.Map<PaymentTypeReadDto>(paymentTypeEntity);
            return Ok(paymentTypeReadDto);
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePaymentType(Guid id)
        {
            var paymentType = await _repository.PaymentType.GetPaymentTypeByIdAsync(id);

            if (paymentType == null)
            {
                _logger.LogError($"PaymentType with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.PaymentType.DeletePaymentTypeAsync(paymentType);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}

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
    public class PaymentsController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public PaymentsController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentReadDto>>> GetAllPayments([FromQuery] QueryStringParameters paginationParameters)
        {
            var payments = await _repository.Payment.GetAllPaymentsAsync(paginationParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(payments.MetaData));

            _logger.LogInfo($"Returned all payments from database.");

            var paymentsReadDto = _mapper.Map<IEnumerable<PaymentReadDto>>(payments);

            return Ok(paymentsReadDto);
        }



        [HttpGet("{id}", Name = "PaymentById")]
        public async Task<ActionResult<PaymentReadDto>> GetPaymentById(Guid id)
        {
            var payment = await _repository.Payment.GetPaymentByIdAsync(id);

            if (payment == null)
            {
                _logger.LogError($"Payment with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned paymentWriteDto with id: {id}");

                var paymentReadDto = _mapper.Map<PaymentReadDto>(payment);
                
                return Ok(paymentReadDto);
            }
        }



        [HttpPost]
        public async Task<ActionResult<PaymentReadDto>> CreatePayment([FromBody] PaymentWriteDto payment)
        {
            if (payment == null)
            {
                _logger.LogError("Payment object sent from payment is null.");
                return BadRequest("Payment object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid paymentWriteDto object sent from payment.");
                return BadRequest("Invalid model object");
            }

            var paymentEntity = _mapper.Map<Payment>(payment);

            if (await _repository.Payment.PaymentExistAsync(paymentEntity))
            {
                ModelState.AddModelError("", "This Payment exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.Payment.CreatePaymentAsync(paymentEntity);
            await _repository.SaveAsync();

            var paymentReadDto = _mapper.Map<PaymentReadDto>(paymentEntity);
            return CreatedAtRoute("PaymentById", new { id = paymentReadDto.Id }, paymentReadDto);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<PaymentReadDto>> UpdatePayment(Guid id, [FromBody] PaymentWriteDto paymentWriteDto)
        {
            if (paymentWriteDto == null)
            {
                _logger.LogError("Payment object sent from payment is null.");
                return BadRequest("Payment object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid paymentWriteDto object sent from payment.");
                return BadRequest("Invalid model object");
            }

            var paymentEntity = await _repository.Payment.GetPaymentByIdAsync(id);
            if (paymentEntity == null)
            {
                _logger.LogError($"Payment with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(paymentWriteDto, paymentEntity);


            await _repository.Payment.UpdatePaymentAsync(paymentEntity);
            await _repository.SaveAsync();

            var paymentReadDto = _mapper.Map<PaymentReadDto>(paymentEntity);
            return Ok(paymentReadDto);
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePayment(Guid id)
        {
            var payment = await _repository.Payment.GetPaymentByIdAsync(id);

            if (payment == null)
            {
                _logger.LogError($"Payment with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.Payment.DeletePaymentAsync(payment);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}

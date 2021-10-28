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
    public class SubscriptionsController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public SubscriptionsController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscriptionReadDto>>> GetAllSubscriptions([FromQuery] SubscriptionParameters queryParameters)
        {
            var subscriptions = await _repository.Subscription.GetAllSubscriptionsAsync(queryParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(subscriptions.MetaData));

            _logger.LogInfo($"Returned all subscriptions from database.");

            var subscriptionsReadDto = _mapper.Map<IEnumerable<SubscriptionReadDto>>(subscriptions);

            return Ok(subscriptionsReadDto);
        }



        [HttpGet("{id}", Name = "SubscriptionById")]
        public async Task<ActionResult<SubscriptionReadDto>> GetSubscriptionById(Guid id)
        {
            var subscription = await _repository.Subscription.GetSubscriptionByIdAsync(id);

            if (subscription == null)
            {
                _logger.LogError($"Subscription with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned subscriptionWriteDto with id: {id}");

                var subscriptionReadDto = _mapper.Map<SubscriptionReadDto>(subscription);
                
                return Ok(subscriptionReadDto);
            }
        }



        [HttpPost]
        public async Task<ActionResult<SubscriptionReadDto>> CreateSubscription([FromBody] SubscriptionWriteDto subscription)
        {
            if (subscription == null)
            {
                _logger.LogError("Subscription object sent from subscription is null.");
                return BadRequest("Subscription object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid subscriptionWriteDto object sent from subscription.");
                return BadRequest("Invalid model object");
            }

            //If the AppUserId is not provided, then affect the currenct logged In User Id
            if (string.IsNullOrWhiteSpace(subscription.AppUserId)) subscription.AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var subscriptionEntity = _mapper.Map<Subscription>(subscription);

            if (await _repository.Subscription.SubscriptionExistAsync(subscriptionEntity))
            {
                ModelState.AddModelError("", "This Subscription exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.Subscription.CreateSubscriptionAsync(subscriptionEntity);
            await _repository.SaveAsync();

            var subscriptionReadDto = _mapper.Map<SubscriptionReadDto>(subscriptionEntity);
            return CreatedAtRoute("SubscriptionById", new { id = subscriptionReadDto.Id }, subscriptionReadDto);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<SubscriptionReadDto>> UpdateSubscription(Guid id, [FromBody] SubscriptionWriteDto subscriptionWriteDto)
        {
            if (subscriptionWriteDto == null)
            {
                _logger.LogError("Subscription object sent from subscription is null.");
                return BadRequest("Subscription object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid subscriptionWriteDto object sent from subscription.");
                return BadRequest("Invalid model object");
            }

            var subscriptionEntity = await _repository.Subscription.GetSubscriptionByIdAsync(id);
            if (subscriptionEntity == null)
            {
                _logger.LogError($"Subscription with id: {id}, hasn't been found.");
                return NotFound();
            }

            //If the AppUserId is not provided, then affect the currenct logged In User Id
            if (string.IsNullOrWhiteSpace(subscriptionWriteDto.AppUserId)) subscriptionWriteDto.AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _mapper.Map(subscriptionWriteDto, subscriptionEntity);

            await _repository.Subscription.UpdateSubscriptionAsync(subscriptionEntity);
            await _repository.SaveAsync();

            var subscriptionReadDto = _mapper.Map<SubscriptionReadDto>(subscriptionEntity);
            return Ok(subscriptionReadDto);
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSubscription(Guid id)
        {
            var subscription = await _repository.Subscription.GetSubscriptionByIdAsync(id);

            if (subscription == null)
            {
                _logger.LogError($"Subscription with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.Subscription.DeleteSubscriptionAsync(subscription);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}

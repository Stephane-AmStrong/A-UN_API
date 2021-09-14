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
    public class PartnersController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public PartnersController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<PartnerReadDto>>> GetAllPartners([FromQuery] QueryStringParameters paginationParameters)
        {
            var partners = await _repository.Partner.GetAllPartnersAsync(paginationParameters);

            var metadata = new
            {
                partners.TotalCount,
                partners.PageSize,
                partners.CurrentPage,
                partners.TotalPages,
                partners.HasNext,
                partners.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            _logger.LogInfo($"Returned all partners from database.");

            var partnersReadDto = _mapper.Map<IEnumerable<PartnerReadDto>>(partners);

            return Ok(partnersReadDto);
        }



        [HttpGet("{id}", Name = "PartnerById")]
        public async Task<ActionResult<PartnerReadDto>> GetPartnerById(Guid id)
        {
            var partner = await _repository.Partner.GetPartnerByIdAsync(id);

            if (partner == null)
            {
                _logger.LogError($"Partner with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned partnerWriteDto with id: {id}");

                var partnerReadDto = _mapper.Map<PartnerReadDto>(partner);
                
                return Ok(partnerReadDto);
            }
        }



        [HttpPost]
        public async Task<ActionResult<PartnerReadDto>> CreatePartner([FromBody] PartnerWriteDto partner)
        {
            if (partner == null)
            {
                _logger.LogError("Partner object sent from partner is null.");
                return BadRequest("Partner object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid partnerWriteDto object sent from partner.");
                return BadRequest("Invalid model object");
            }

            var partnerEntity = _mapper.Map<Partner>(partner);

            if (await _repository.Partner.PartnerExistAsync(partnerEntity))
            {
                ModelState.AddModelError("", "This Partner exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.Partner.CreatePartnerAsync(partnerEntity);
            await _repository.SaveAsync();

            var partnerReadDto = _mapper.Map<PartnerReadDto>(partnerEntity);
            return CreatedAtRoute("PartnerById", new { id = partnerReadDto.Id }, partnerReadDto);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<PartnerReadDto>> UpdatePartner(Guid id, [FromBody] PartnerWriteDto partnerWriteDto)
        {
            if (partnerWriteDto == null)
            {
                _logger.LogError("Partner object sent from partner is null.");
                return BadRequest("Partner object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid partnerWriteDto object sent from partner.");
                return BadRequest("Invalid model object");
            }

            var partnerEntity = await _repository.Partner.GetPartnerByIdAsync(id);
            if (partnerEntity == null)
            {
                _logger.LogError($"Partner with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(partnerWriteDto, partnerEntity);


            await _repository.Partner.UpdatePartnerAsync(partnerEntity);
            await _repository.SaveAsync();

            var partnerReadDto = _mapper.Map<PartnerReadDto>(partnerEntity);
            return Ok(partnerReadDto);
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePartner(Guid id)
        {
            var partner = await _repository.Partner.GetPartnerByIdAsync(id);

            if (partner == null)
            {
                _logger.LogError($"Partner with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.Partner.DeletePartnerAsync(partner);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}

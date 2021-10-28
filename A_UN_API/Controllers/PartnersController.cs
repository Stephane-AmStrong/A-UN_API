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
    public class PartnersController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly string _baseURL;

        public PartnersController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _repository.Path = "/pictures/Partner";
            _baseURL = string.Concat(httpContextAccessor.HttpContext.Request.Scheme, "://", httpContextAccessor.HttpContext.Request.Host);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<PartnerReadDto>>> GetAllPartners([FromQuery] PartnerParameters partnerParameters)
        {
            var partners = await _repository.Partner.GetAllPartnersAsync(partnerParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(partners.MetaData));

            _logger.LogInfo($"Returned all partners from database.");

            var partnersReadDto = _mapper.Map<IEnumerable<PartnerReadDto>>(partners);

            partnersReadDto.ToList().ForEach(partnerReadDto =>
            {
                if (!string.IsNullOrWhiteSpace(partnerReadDto.ImgLink)) partnerReadDto.ImgLink = $"{_baseURL}{partnerReadDto.ImgLink}";
            });

            return Ok(partnersReadDto);
        }



        [HttpGet("{id}", Name = "GetPartnerById")]
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

                if (!string.IsNullOrWhiteSpace(partnerReadDto.ImgLink)) partnerReadDto.ImgLink = $"{_baseURL}{partnerReadDto.ImgLink}";

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

            if (!string.IsNullOrWhiteSpace(partnerReadDto.ImgLink)) partnerReadDto.ImgLink = $"{_baseURL}{partnerReadDto.ImgLink}";

            return Ok(partnerReadDto);
        }



        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialBranchUpdate(Guid Id, JsonPatchDocument<BranchWriteDto> patchDoc)
        {
            var branchModelFromRepository = await _repository.Branch.GetBranchByIdAsync(Id);
            if (branchModelFromRepository == null) return NotFound();

            var branchToPatch = _mapper.Map<BranchWriteDto>(branchModelFromRepository);
            patchDoc.ApplyTo(branchToPatch, ModelState);

            if (!TryValidateModel(branchToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(branchToPatch, branchModelFromRepository);

            await _repository.Branch.UpdateBranchAsync(branchModelFromRepository);

            await _repository.SaveAsync();

            return NoContent();
        }



        [HttpPut("{id}/upload-picture")]
        public async Task<ActionResult<PartnerReadDto>> UploadPicture(Guid id, [FromForm] IFormFile file)
        {
            var partnerEntity = await _repository.Partner.GetPartnerByIdAsync(id);
            if (partnerEntity == null) return NotFound();

            if (file != null)
            {
                _repository.File.FilePath = id.ToString();

                var uploadResult = await _repository.File.UploadFile(file);

                if (uploadResult == null)
                {
                    ModelState.AddModelError("", "something went wrong when uploading the picture");
                    return ValidationProblem(ModelState);
                }
                else
                {
                    partnerEntity.ImgLink = uploadResult;
                }
            }

            await _repository.Partner.UpdatePartnerAsync(partnerEntity);

            await _repository.SaveAsync();

            var partnerReadDto = _mapper.Map<PartnerReadDto>(partnerEntity);

            if (!string.IsNullOrWhiteSpace(partnerReadDto.ImgLink)) partnerReadDto.ImgLink = $"{_baseURL}{partnerReadDto.ImgLink}";

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

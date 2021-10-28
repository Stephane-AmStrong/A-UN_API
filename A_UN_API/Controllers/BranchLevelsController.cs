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
    public class BranchLevelsController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly string _baseURL;

        public BranchLevelsController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _repository.Path = "/pictures/BranchLevel";
            _baseURL = string.Concat(httpContextAccessor.HttpContext.Request.Scheme, "://", httpContextAccessor.HttpContext.Request.Host);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<BranchLevelReadDto>>> GetAllBranchLevels([FromQuery] BranchLevelParameters queryParameters)
        {
            var branchLevels = await _repository.BranchLevel.GetAllBranchLevelsAsync(queryParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(branchLevels.MetaData));

            _logger.LogInfo($"Returned all branchLevels from database.");

            var branchLevelsReadDto = _mapper.Map<IEnumerable<BranchLevelReadDto>>(branchLevels);

            branchLevelsReadDto.ToList().ForEach(branchLevelReadDto =>
            {
                if (!string.IsNullOrWhiteSpace(branchLevelReadDto.ImgLink)) branchLevelReadDto.ImgLink = $"{_baseURL}{branchLevelReadDto.ImgLink}";
            });

            return Ok(branchLevelsReadDto);
        }



        [HttpGet("{id}", Name = "BranchLevelById")]
        public async Task<ActionResult<BranchLevelReadDto>> GetBranchLevelById(Guid id)
        {
            var branchLevel = await _repository.BranchLevel.GetBranchLevelByIdAsync(id);

            if (branchLevel == null)
            {
                _logger.LogError($"BranchLevel with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned branchLevelWriteDto with id: {id}");

                var branchLevelReadDto = _mapper.Map<BranchLevelReadDto>(branchLevel);

                if (!string.IsNullOrWhiteSpace(branchLevelReadDto.ImgLink)) branchLevelReadDto.ImgLink = $"{_baseURL}{branchLevelReadDto.ImgLink}";

                return Ok(branchLevelReadDto);
            }
        }



        [HttpPost]
        public async Task<ActionResult<BranchLevelReadDto>> CreateBranchLevel([FromBody] BranchLevelWriteDto branchLevel)
        {
            if (branchLevel == null)
            {
                _logger.LogError("BranchLevel object sent from branchLevel is null.");
                return BadRequest("BranchLevel object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid branchLevelWriteDto object sent from branchLevel.");
                return BadRequest("Invalid model object");
            }

            var branchLevelEntity = _mapper.Map<BranchLevel>(branchLevel);

            if (await _repository.BranchLevel.BranchLevelExistAsync(branchLevelEntity))
            {
                ModelState.AddModelError("", "This BranchLevel exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.BranchLevel.CreateBranchLevelAsync(branchLevelEntity);
            await _repository.SaveAsync();

            var branchLevelReadDto = _mapper.Map<BranchLevelReadDto>(branchLevelEntity);
            return CreatedAtRoute("BranchLevelById", new { id = branchLevelReadDto.Id }, branchLevelReadDto);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<BranchLevelReadDto>> UpdateBranchLevel(Guid id, [FromBody] BranchLevelWriteDto branchLevelWriteDto)
        {
            if (branchLevelWriteDto == null)
            {
                _logger.LogError("BranchLevel object sent from branchLevel is null.");
                return BadRequest("BranchLevel object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid branchLevelWriteDto object sent from branchLevel.");
                return BadRequest("Invalid model object");
            }

            var branchLevelEntity = await _repository.BranchLevel.GetBranchLevelByIdAsync(id);
            if (branchLevelEntity == null)
            {
                _logger.LogError($"BranchLevel with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(branchLevelWriteDto, branchLevelEntity);


            await _repository.BranchLevel.UpdateBranchLevelAsync(branchLevelEntity);
            await _repository.SaveAsync();

            var branchLevelReadDto = _mapper.Map<BranchLevelReadDto>(branchLevelEntity);

            if (!string.IsNullOrWhiteSpace(branchLevelReadDto.ImgLink)) branchLevelReadDto.ImgLink = $"{_baseURL}{branchLevelReadDto.ImgLink}";

            return Ok(branchLevelReadDto);
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
        public async Task<ActionResult<BranchLevelReadDto>> UploadPicture(Guid id, [FromForm] IFormFile file)
        {
            var branchLevelEntity = await _repository.BranchLevel.GetBranchLevelByIdAsync(id);
            if (branchLevelEntity == null) return NotFound();

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
                    branchLevelEntity.ImgLink = uploadResult;
                }
            }

            await _repository.BranchLevel.UpdateBranchLevelAsync(branchLevelEntity);

            await _repository.SaveAsync();

            var branchLevelReadDto = _mapper.Map<BranchLevelReadDto>(branchLevelEntity);

            if (!string.IsNullOrWhiteSpace(branchLevelReadDto.ImgLink)) branchLevelReadDto.ImgLink = $"{_baseURL}{branchLevelReadDto.ImgLink}";

            return Ok(branchLevelReadDto);
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBranchLevel(Guid id)
        {
            var branchLevel = await _repository.BranchLevel.GetBranchLevelByIdAsync(id);

            if (branchLevel == null)
            {
                _logger.LogError($"BranchLevel with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.BranchLevel.DeleteBranchLevelAsync(branchLevel);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}

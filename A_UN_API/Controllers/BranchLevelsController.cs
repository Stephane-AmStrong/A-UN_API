﻿using AutoMapper;
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
    public class BranchLevelsController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public BranchLevelsController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<BranchLevelReadDto>>> GetAllBranchLevels([FromQuery] QueryStringParameters paginationParameters)
        {
            var branchLevels = await _repository.BranchLevel.GetAllBranchLevelsAsync(paginationParameters);

            var metadata = new
            {
                branchLevels.TotalCount,
                branchLevels.PageSize,
                branchLevels.CurrentPage,
                branchLevels.TotalPages,
                branchLevels.HasNext,
                branchLevels.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            _logger.LogInfo($"Returned all branchLevels from database.");

            var branchLevelsReadDto = _mapper.Map<IEnumerable<BranchLevelReadDto>>(branchLevels);

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
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
    public class BranchesController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public BranchesController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<BranchReadDto>>> GetAllBranches([FromQuery] QueryStringParameters paginationParameters)
        {
            var branches = await _repository.Branch.GetAllBranchesAsync(paginationParameters);

            var metadata = new
            {
                branches.TotalCount,
                branches.PageSize,
                branches.CurrentPage,
                branches.TotalPages,
                branches.HasNext,
                branches.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            _logger.LogInfo($"Returned all branches from database.");

            var branchesReadDto = _mapper.Map<IEnumerable<BranchReadDto>>(branches);

            return Ok(branchesReadDto);
        }



        [HttpGet("{id}", Name = "BranchById")]
        public async Task<ActionResult<BranchReadDto>> GetBranchById(Guid id)
        {
            var branch = await _repository.Branch.GetBranchByIdAsync(id);

            if (branch == null)
            {
                _logger.LogError($"Branch with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned branchWriteDto with id: {id}");

                var branchReadDto = _mapper.Map<BranchReadDto>(branch);
                
                return Ok(branchReadDto);
            }
        }



        [HttpPost]
        public async Task<ActionResult<BranchReadDto>> CreateBranch([FromBody] BranchWriteDto branch)
        {
            if (branch == null)
            {
                _logger.LogError("Branch object sent from branch is null.");
                return BadRequest("Branch object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid branchWriteDto object sent from branch.");
                return BadRequest("Invalid model object");
            }

            var branchEntity = _mapper.Map<Branch>(branch);

            if (await _repository.Branch.BranchExistAsync(branchEntity))
            {
                ModelState.AddModelError("", "This Branch exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.Branch.CreateBranchAsync(branchEntity);
            await _repository.SaveAsync();

            var branchReadDto = _mapper.Map<BranchReadDto>(branchEntity);
            return CreatedAtRoute("BranchById", new { id = branchReadDto.Id }, branchReadDto);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<BranchReadDto>> UpdateBranch(Guid id, [FromBody] BranchWriteDto branchWriteDto)
        {
            if (branchWriteDto == null)
            {
                _logger.LogError("Branch object sent from branch is null.");
                return BadRequest("Branch object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid branchWriteDto object sent from branch.");
                return BadRequest("Invalid model object");
            }

            var branchEntity = await _repository.Branch.GetBranchByIdAsync(id);
            if (branchEntity == null)
            {
                _logger.LogError($"Branch with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(branchWriteDto, branchEntity);


            await _repository.Branch.UpdateBranchAsync(branchEntity);
            await _repository.SaveAsync();

            var branchReadDto = _mapper.Map<BranchReadDto>(branchEntity);
            return Ok(branchReadDto);
        }


        // PATCH api/branches/{id}
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
        public async Task<ActionResult> DeleteBranch(Guid id)
        {
            var branch = await _repository.Branch.GetBranchByIdAsync(id);

            if (branch == null)
            {
                _logger.LogError($"Branch with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.Branch.DeleteBranchAsync(branch);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}

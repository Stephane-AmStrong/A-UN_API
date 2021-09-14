using A_UN_API.Helpers;
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
using System.Security.Claims;
using System.Threading.Tasks;


namespace GesProdAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WorkstationsController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public WorkstationsController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [MultiplePoliciesAuthorize("readWorkstationPolicy; writeWorkstationPolicy")]
        public async Task<ActionResult<IEnumerable<WorkstationReadDto>>> GetAllWorkstations([FromQuery] QueryStringParameters paginationParameters)
        {
            var workstations = await _repository.Workstation.GetAllWorkstationsAsync(paginationParameters);

            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var metadata = new
            {
                workstations.TotalCount,
                workstations.PageSize,
                workstations.CurrentPage,
                workstations.TotalPages,
                workstations.HasNext,
                workstations.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            _logger.LogInfo($"Returned all workstations from database.");

            var workstationsReadDto = _mapper.Map<IEnumerable<WorkstationReadDto>>(workstations);

            return Ok(workstationsReadDto);
        }



        [HttpGet("{id}", Name = "WorkstationById")]
        public async Task<ActionResult<WorkstationReadDto>> GetWorkstationById(Guid id)
        {
            var workstation = await _repository.Workstation.GetWorkstationByIdAsync(id);

            if (workstation == null)
            {
                _logger.LogError($"Workstation with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned workstationWriteDto with id: {id}");

                var workstationReadDto = _mapper.Map<WorkstationReadDto>(workstation);
                
                return Ok(workstationReadDto);
            }
        }



        [HttpPost]
        public async Task<ActionResult<WorkstationReadDto>> CreateWorkstation([FromBody] WorkstationWriteDto workstation)
        {
            if (workstation == null)
            {
                _logger.LogError("Workstation object sent from workstation is null.");
                return BadRequest("Workstation object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid workstationWriteDto object sent from workstation.");
                return BadRequest("Invalid model object");
            }

            var workstationEntity = _mapper.Map<Workstation>(workstation);

            if (await _repository.Workstation.WorkstationExistAsync(workstationEntity))
            {
                ModelState.AddModelError("", "This Workstation exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.Workstation.CreateWorkstationAsync(workstationEntity);
            await _repository.SaveAsync();

            var workstationReadDto = _mapper.Map<WorkstationReadDto>(workstationEntity);
            return CreatedAtRoute("WorkstationById", new { id = workstationReadDto.Id }, workstationReadDto);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<WorkstationReadDto>> UpdateWorkstation(Guid id, [FromBody] WorkstationWriteDto workstationWriteDto)
        {
            if (workstationWriteDto == null)
            {
                _logger.LogError("Workstation object sent from workstation is null.");
                return BadRequest("Workstation object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid workstationWriteDto object sent from workstation.");
                return BadRequest("Invalid model object");
            }

            var workstationEntity = await _repository.Workstation.GetWorkstationByIdAsync(id);
            if (workstationEntity == null)
            {
                _logger.LogError($"Workstation with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(workstationWriteDto, workstationEntity);


            await _repository.Workstation.UpdateWorkstationAsync(workstationEntity);
            await _repository.SaveAsync();

            var workstationReadDto = _mapper.Map<WorkstationReadDto>(workstationEntity);
            return Ok(workstationReadDto);
        }




        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialWorkstationUpdate(Guid Id, JsonPatchDocument<WorkstationWriteDto> patchDoc)
        {
            var workstationModelFromRepository = await _repository.Workstation.GetWorkstationByIdAsync(Id);
            if (workstationModelFromRepository == null) return NotFound();

            var workstationToPatch = _mapper.Map<WorkstationWriteDto>(workstationModelFromRepository);
            patchDoc.ApplyTo(workstationToPatch, ModelState);

            if (!TryValidateModel(workstationToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(workstationToPatch, workstationModelFromRepository);

            await _repository.Workstation.UpdateWorkstationAsync(workstationModelFromRepository);
                             
            await _repository.SaveAsync();

            return NoContent();
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWorkstation(Guid id)
        {
            var workstation = await _repository.Workstation.GetWorkstationByIdAsync(id);

            if (workstation == null)
            {
                _logger.LogError($"Workstation with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.Workstation.DeleteWorkstationAsync(workstation);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}

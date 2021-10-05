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
    public class ObjectivesController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public ObjectivesController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ObjectiveReadDto>>> GetAllObjectives([FromQuery] QueryStringParameters paginationParameters)
        {
            var objectives = await _repository.Objective.GetAllObjectivesAsync(paginationParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(objectives.MetaData));

            _logger.LogInfo($"Returned all objectives from database.");

            var objectivesReadDto = _mapper.Map<IEnumerable<ObjectiveReadDto>>(objectives);

            return Ok(objectivesReadDto);
        }



        [HttpGet("{id}", Name = "ObjectiveById")]
        public async Task<ActionResult<ObjectiveReadDto>> GetObjectiveById(Guid id)
        {
            var objective = await _repository.Objective.GetObjectiveByIdAsync(id);

            if (objective == null)
            {
                _logger.LogError($"Objective with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned objectiveWriteDto with id: {id}");

                var objectiveReadDto = _mapper.Map<ObjectiveReadDto>(objective);
                
                return Ok(objectiveReadDto);
            }
        }



        [HttpPost]
        public async Task<ActionResult<ObjectiveReadDto>> CreateObjective([FromBody] ObjectiveWriteDto objective)
        {
            if (objective == null)
            {
                _logger.LogError("Objective object sent from objective is null.");
                return BadRequest("Objective object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid objectiveWriteDto object sent from objective.");
                return BadRequest("Invalid model object");
            }

            var objectiveEntity = _mapper.Map<Objective>(objective);

            if (await _repository.Objective.ObjectiveExistAsync(objectiveEntity))
            {
                ModelState.AddModelError("", "This Objective exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.Objective.CreateObjectiveAsync(objectiveEntity);
            await _repository.SaveAsync();

            var objectiveReadDto = _mapper.Map<ObjectiveReadDto>(objectiveEntity);
            return CreatedAtRoute("ObjectiveById", new { id = objectiveReadDto.Id }, objectiveReadDto);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<ObjectiveReadDto>> UpdateObjective(Guid id, [FromBody] ObjectiveWriteDto objectiveWriteDto)
        {
            if (objectiveWriteDto == null)
            {
                _logger.LogError("Objective object sent from objective is null.");
                return BadRequest("Objective object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid objectiveWriteDto object sent from objective.");
                return BadRequest("Invalid model object");
            }

            var objectiveEntity = await _repository.Objective.GetObjectiveByIdAsync(id);
            if (objectiveEntity == null)
            {
                _logger.LogError($"Objective with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(objectiveWriteDto, objectiveEntity);


            await _repository.Objective.UpdateObjectiveAsync(objectiveEntity);
            await _repository.SaveAsync();

            var objectiveReadDto = _mapper.Map<ObjectiveReadDto>(objectiveEntity);
            return Ok(objectiveReadDto);
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteObjective(Guid id)
        {
            var objective = await _repository.Objective.GetObjectiveByIdAsync(id);

            if (objective == null)
            {
                _logger.LogError($"Objective with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.Objective.DeleteObjectiveAsync(objective);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}

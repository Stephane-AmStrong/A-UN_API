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
    public class FormationLevelsController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly string _baseURL;

        public FormationLevelsController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _repository.Path = "/pictures/FormationLevel";
            _baseURL = string.Concat(httpContextAccessor.HttpContext.Request.Scheme, "://", httpContextAccessor.HttpContext.Request.Host);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<FormationLevelReadDto>>> GetFormationLevels([FromQuery] FormationLevelParameters queryParameters)
        {
            var formationLevels = await _repository.FormationLevel.GetFormationLevelsAsync(queryParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(formationLevels.MetaData));

            _logger.LogInfo($"Returned all formationLevels from database.");

            var formationLevelsReadDto = _mapper.Map<IEnumerable<FormationLevelReadDto>>(formationLevels);

            formationLevelsReadDto.ToList().ForEach(formationLevelReadDto =>
            {
                if (!string.IsNullOrWhiteSpace(formationLevelReadDto.ImgLink)) formationLevelReadDto.ImgLink = $"{_baseURL}{formationLevelReadDto.ImgLink}";
            });

            return Ok(formationLevelsReadDto);
        }



        [HttpGet("{id}", Name = "FormationLevelById")]
        public async Task<ActionResult<FormationLevelReadDto>> GetFormationLevelById(Guid id)
        {
            var formationLevel = await _repository.FormationLevel.GetFormationLevelByIdAsync(id);

            if (formationLevel == null)
            {
                _logger.LogError($"FormationLevel with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned formationLevelWriteDto with id: {id}");

                var formationLevelReadDto = _mapper.Map<FormationLevelReadDto>(formationLevel);

                if (!string.IsNullOrWhiteSpace(formationLevelReadDto.ImgLink)) formationLevelReadDto.ImgLink = $"{_baseURL}{formationLevelReadDto.ImgLink}";

                return Ok(formationLevelReadDto);
            }
        }



        [HttpPost]
        public async Task<ActionResult<FormationLevelReadDto>> CreateFormationLevel([FromBody] FormationLevelWriteDto formationLevel)
        {
            if (formationLevel == null)
            {
                _logger.LogError("FormationLevel object sent from formationLevel is null.");
                return BadRequest("FormationLevel object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid formationLevelWriteDto object sent from formationLevel.");
                return BadRequest("Invalid model object");
            }

            var formationLevelEntity = _mapper.Map<FormationLevel>(formationLevel);

            if (await _repository.FormationLevel.FormationLevelExistAsync(formationLevelEntity))
            {
                ModelState.AddModelError("", "This FormationLevel exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.FormationLevel.CreateFormationLevelAsync(formationLevelEntity);
            await _repository.SaveAsync();

            var formationLevelReadDto = _mapper.Map<FormationLevelReadDto>(formationLevelEntity);
            return CreatedAtRoute("FormationLevelById", new { id = formationLevelReadDto.Id }, formationLevelReadDto);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<FormationLevelReadDto>> UpdateFormationLevel(Guid id, [FromBody] FormationLevelWriteDto formationLevelWriteDto)
        {
            if (formationLevelWriteDto == null)
            {
                _logger.LogError("FormationLevel object sent from formationLevel is null.");
                return BadRequest("FormationLevel object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid formationLevelWriteDto object sent from formationLevel.");
                return BadRequest("Invalid model object");
            }

            var formationLevelEntity = await _repository.FormationLevel.GetFormationLevelByIdAsync(id);
            if (formationLevelEntity == null)
            {
                _logger.LogError($"FormationLevel with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(formationLevelWriteDto, formationLevelEntity);


            await _repository.FormationLevel.UpdateFormationLevelAsync(formationLevelEntity);
            await _repository.SaveAsync();

            var formationLevelReadDto = _mapper.Map<FormationLevelReadDto>(formationLevelEntity);

            if (!string.IsNullOrWhiteSpace(formationLevelReadDto.ImgLink)) formationLevelReadDto.ImgLink = $"{_baseURL}{formationLevelReadDto.ImgLink}";

            return Ok(formationLevelReadDto);
        }




        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialFormationUpdate(Guid Id, JsonPatchDocument<FormationWriteDto> patchDoc)
        {
            var formationModelFromRepository = await _repository.Formation.GetFormationByIdAsync(Id);
            if (formationModelFromRepository == null) return NotFound();

            var formationToPatch = _mapper.Map<FormationWriteDto>(formationModelFromRepository);
            patchDoc.ApplyTo(formationToPatch, ModelState);

            if (!TryValidateModel(formationToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(formationToPatch, formationModelFromRepository);

            await _repository.Formation.UpdateFormationAsync(formationModelFromRepository);

            await _repository.SaveAsync();

            return NoContent();
        }



        [HttpPut("{id}/upload-picture")]
        public async Task<ActionResult<FormationLevelReadDto>> UploadPicture(Guid id, [FromForm] IFormFile file)
        {
            var formationLevelEntity = await _repository.FormationLevel.GetFormationLevelByIdAsync(id);
            if (formationLevelEntity == null) return NotFound();

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
                    formationLevelEntity.ImgLink = uploadResult;
                }
            }

            await _repository.FormationLevel.UpdateFormationLevelAsync(formationLevelEntity);

            await _repository.SaveAsync();

            var formationLevelReadDto = _mapper.Map<FormationLevelReadDto>(formationLevelEntity);

            if (!string.IsNullOrWhiteSpace(formationLevelReadDto.ImgLink)) formationLevelReadDto.ImgLink = $"{_baseURL}{formationLevelReadDto.ImgLink}";

            return Ok(formationLevelReadDto);
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFormationLevel(Guid id)
        {
            var formationLevel = await _repository.FormationLevel.GetFormationLevelByIdAsync(id);

            if (formationLevel == null)
            {
                _logger.LogError($"FormationLevel with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.FormationLevel.DeleteFormationLevelAsync(formationLevel);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}

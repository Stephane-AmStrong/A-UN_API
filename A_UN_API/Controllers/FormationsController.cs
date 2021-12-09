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
    public class FormationsController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly string _baseURL;

        public FormationsController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _repository.Path = "/pictures/Formation";
            _baseURL = string.Concat(httpContextAccessor.HttpContext.Request.Scheme, "://", httpContextAccessor.HttpContext.Request.Host);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<FormationReadDto>>> GetFormations([FromQuery] FormationParameters queryParameters)
        {
            var formations = await _repository.Formation.GetFormationsAsync(queryParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(formations.MetaData));

            _logger.LogInfo($"Returned all formations from database.");

            var formationsReadDto = _mapper.Map<IEnumerable<FormationReadDto>>(formations);

            formationsReadDto.ToList().ForEach(formationReadDto =>
            {
                if (!string.IsNullOrWhiteSpace(formationReadDto.ImgLink)) formationReadDto.ImgLink = $"{_baseURL}{formationReadDto.ImgLink}";
            });

            return Ok(formationsReadDto);
        }



        [HttpGet("{id}", Name = "FormationById")]
        public async Task<ActionResult<FormationReadDto>> GetFormationById(Guid id)
        {
            var formation = await _repository.Formation.GetFormationByIdAsync(id);

            if (formation == null)
            {
                _logger.LogError($"Formation with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned formationWriteDto with id: {id}");

                var formationReadDto = _mapper.Map<FormationReadDto>(formation);

                if (!string.IsNullOrWhiteSpace(formationReadDto.ImgLink)) formationReadDto.ImgLink = $"{_baseURL}{formationReadDto.ImgLink}";

                return Ok(formationReadDto);
            }
        }



        [HttpPost]
        public async Task<ActionResult<FormationReadDto>> CreateFormation([FromBody] FormationWriteDto formation)
        {
            if (formation == null)
            {
                _logger.LogError("Formation object sent from formation is null.");
                return BadRequest("Formation object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid formationWriteDto object sent from formation.");
                return BadRequest("Invalid model object");
            }

            var formationEntity = _mapper.Map<Formation>(formation);

            if (await _repository.Formation.FormationExistAsync(formationEntity))
            {
                ModelState.AddModelError("", "This Formation exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.Formation.CreateFormationAsync(formationEntity);
            await _repository.SaveAsync();

            var formationReadDto = _mapper.Map<FormationReadDto>(formationEntity);
            return CreatedAtRoute("FormationById", new { id = formationReadDto.Id }, formationReadDto);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<FormationReadDto>> UpdateFormation(Guid id, [FromBody] FormationWriteDto formationWriteDto)
        {
            if (formationWriteDto == null)
            {
                _logger.LogError("Formation object sent from formation is null.");
                return BadRequest("Formation object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid formationWriteDto object sent from formation.");
                return BadRequest("Invalid model object");
            }

            var formationEntity = await _repository.Formation.GetFormationByIdAsync(id);
            if (formationEntity == null)
            {
                _logger.LogError($"Formation with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(formationWriteDto, formationEntity);


            await _repository.Formation.UpdateFormationAsync(formationEntity);
            await _repository.SaveAsync();

            var formationReadDto = _mapper.Map<FormationReadDto>(formationEntity);

            if (!string.IsNullOrWhiteSpace(formationReadDto.ImgLink)) formationReadDto.ImgLink = $"{_baseURL}{formationReadDto.ImgLink}";

            return Ok(formationReadDto);
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
        public async Task<ActionResult<FormationReadDto>> UploadPicture(Guid id, [FromForm] IFormFile file)
        {
            var formationEntity = await _repository.Formation.GetFormationByIdAsync(id);
            if (formationEntity == null) return NotFound();

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
                    formationEntity.ImgLink = uploadResult;
                }
            }

            await _repository.Formation.UpdateFormationAsync(formationEntity);

            await _repository.SaveAsync();

            var formationReadDto = _mapper.Map<FormationReadDto>(formationEntity);

            if (!string.IsNullOrWhiteSpace(formationReadDto.ImgLink)) formationReadDto.ImgLink = $"{_baseURL}{formationReadDto.ImgLink}";

            return Ok(formationReadDto);
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFormation(Guid id)
        {
            var formation = await _repository.Formation.GetFormationByIdAsync(id);

            if (formation == null)
            {
                _logger.LogError($"Formation with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.Formation.DeleteFormationAsync(formation);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}

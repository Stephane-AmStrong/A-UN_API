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
    public class AppUsersController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly string _baseURL;

        public AppUsersController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _repository.Path = "/pictures/AppUser";
            _baseURL = string.Concat(httpContextAccessor.HttpContext.Request.Scheme, "://", httpContextAccessor.HttpContext.Request.Host);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUserReadDto>>> GetAllAppUsers([FromQuery] AppUserParameters queryParameters)
        {
            var appUsers = await _repository.AppUser.GetAllAppUsersAsync(queryParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(appUsers.MetaData));

            _logger.LogInfo($"Returned all appUsers from database.");

            var appUsersReadDto = _mapper.Map<IEnumerable<AppUserReadDto>>(appUsers);

            appUsersReadDto.ToList().ForEach(appUserReadDto =>
            {
                if (!string.IsNullOrWhiteSpace(appUserReadDto.ImgLink)) appUserReadDto.ImgLink = $"{_baseURL}{appUserReadDto.ImgLink}";
            });

            return Ok(appUsersReadDto);
        }



        [HttpGet("{id}", Name = "AppUserById")]
        public async Task<ActionResult<AppUserReadDto>> GetAppUserById(string id)
        {
            var appUser = await _repository.AppUser.GetAppUserByIdAsync(id);

            if (appUser == null)
            {
                _logger.LogError($"AppUser with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned appUserWriteDto with id: {id}");

                var appUserReadDto = _mapper.Map<AppUserReadDto>(appUser);

                if (!string.IsNullOrWhiteSpace(appUserReadDto.ImgLink)) appUserReadDto.ImgLink = $"{_baseURL}{appUserReadDto.ImgLink}";

                return Ok(appUserReadDto);
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<AppUserReadDto>> UpdateAppUser(string id, [FromBody] AppUserWriteDto appUserWriteDto)
        {
            if (appUserWriteDto == null)
            {
                _logger.LogError("AppUser object sent from appUser is null.");
                return BadRequest("AppUser object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid appUserWriteDto object sent from appUser.");
                return BadRequest("Invalid model object");
            }

            var appUserEntity = await _repository.AppUser.GetAppUserByIdAsync(id);
            if (appUserEntity == null)
            {
                _logger.LogError($"AppUser with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(appUserWriteDto, appUserEntity);


            await _repository.AppUser.UpdateAppUserAsync(appUserEntity);
            await _repository.SaveAsync();

            var appUserReadDto = _mapper.Map<AppUserReadDto>(appUserEntity);
            
            if (!string.IsNullOrWhiteSpace(appUserReadDto.ImgLink)) appUserReadDto.ImgLink = $"{_baseURL}{appUserReadDto.ImgLink}";
            
            return Ok(appUserReadDto);
        }




        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialAppUserUpdate(string Id, JsonPatchDocument<AppUserWriteDto> patchDoc)
        {
            var appUserModelFromRepository = await _repository.AppUser.GetAppUserByIdAsync(Id);
            if (appUserModelFromRepository == null) return NotFound();

            var appUserToPatch = _mapper.Map<AppUserWriteDto>(appUserModelFromRepository);
            patchDoc.ApplyTo(appUserToPatch, ModelState);

            if (!TryValidateModel(appUserToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(appUserToPatch, appUserModelFromRepository);

            await _repository.AppUser.UpdateAppUserAsync(appUserModelFromRepository);
                             
            await _repository.SaveAsync();

            return NoContent();
        }




        [HttpPut("{id}/upload-picture")]
        public async Task<ActionResult<AppUserReadDto>> UploadPicture(string id, [FromForm] IFormFile file)
        {
            var appUserEntity = await _repository.AppUser.GetAppUserByIdAsync(id);
            if (appUserEntity == null) return NotFound();

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
                    appUserEntity.ImgLink = uploadResult;
                }
            }

            await _repository.AppUser.UpdateAppUserAsync(appUserEntity);

            await _repository.SaveAsync();

            var appUserReadDto = _mapper.Map<AppUserReadDto>(appUserEntity);

            if (!string.IsNullOrWhiteSpace(appUserReadDto.ImgLink)) appUserReadDto.ImgLink = $"{_baseURL}{appUserReadDto.ImgLink}";

            return Ok(appUserReadDto);
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAppUser(string id)
        {
            var appUser = await _repository.AppUser.GetAppUserByIdAsync(id);

            if (appUser == null)
            {
                _logger.LogError($"AppUser with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.AppUser.DeleteAppUserAsync(appUser);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}

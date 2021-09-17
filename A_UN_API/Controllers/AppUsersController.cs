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
    public class AppUsersController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public AppUsersController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUserReadDto>>> GetAllAppUsers([FromQuery] QueryStringParameters paginationParameters)
        {
            var appUsers = await _repository.AppUser.GetAllAppUsersAsync(paginationParameters);

            var metadata = new
            {
                appUsers.TotalCount,
                appUsers.PageSize,
                appUsers.CurrentPage,
                appUsers.TotalPages,
                appUsers.HasNext,
                appUsers.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            _logger.LogInfo($"Returned all appUsers from database.");

            var appUsersReadDto = _mapper.Map<IEnumerable<AppUserReadDto>>(appUsers);

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

using AutoMapper;
using Contracts;
using Entities.DataTransfertObjects;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GesProdAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationsController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;




        public AuthenticationsController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }




        //GET api/authentications/users/count
        [HttpGet("users/count")]
        [AllowAnonymous]
        public async Task<int> GetUsersCount()
        {
            _logger.LogInfo($"Count users of database.");
            return (await _repository.Authentication.CountUsersAsync());
        }



        //POST api/authentications/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInfo($"Authentication attempt");

                var loginRequest = _mapper.Map<LoginRequest>(loginRequestDto);
                var authenticationResponse = await _repository.Authentication.LoginWithUserNameAsync(loginRequest, loginRequestDto.Password);

                if (authenticationResponse.IsSuccess)
                {
                    _logger.LogInfo($"User Named: {authenticationResponse.UserInfo["Name"]} has logged in successfully");
                    
                    var authenticationResponseReadDto = _mapper.Map<AuthenticationResponseReadDto>(authenticationResponse);

                    return Ok(authenticationResponseReadDto);
                }

                _logger.LogError($"Authentication failed : {authenticationResponse.Message}");
                return NotFound(authenticationResponse.Message);
            }

            return ValidationProblem(ModelState);
        }




        //POST api/authentications/user/registration
        [HttpPost("user/registration")]
        [AllowAnonymous]
        public async Task<ActionResult<AppUserReadDto>> RegisterUser([FromBody] AppUserWriteDto userRegistrationDto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            if (await GetUsersCount() < 1)
            {
                ModelState.AddModelError("", "create an admin account first");
                return ValidationProblem(ModelState);
            }


            _logger.LogInfo($"Registration attempt by : {userRegistrationDto.Firstname } {userRegistrationDto.Name }");

            var userRegistration = _mapper.Map<AppUser>(userRegistrationDto);
            var result = await _repository.Authentication.RegisterUserAsync(userRegistration, userRegistrationDto.Password);

            if (result.IsSuccess)
            {
                var userReadDto = _mapper.Map<AppUserReadDto>(userRegistration);
                _logger.LogInfo($"Registration was successful");

                return Ok(userReadDto);
            }
            else
            {
                foreach (var error in result.ErrorDetails)
                {
                    ModelState.AddModelError(error, error);
                }
                _logger.LogError($"Registration failed ErrorMessage : {result.ErrorDetails}");
                return ValidationProblem(ModelState);
            }
        }





        [HttpPost("admin/registration")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        public async Task<ActionResult<AppUserReadDto>> RegisterAdmin([FromBody] AppUserWriteDto adminRegistrationDto)
        {
            if (await GetUsersCount() >= 1) return BadRequest("No longer available");

            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var workstation = await _repository.Workstation.GetWorkstationByNameAsync("SuperAdmin");
            if (workstation == null) return NotFound("Workstation not found");

            var admin = _mapper.Map<AppUser>(adminRegistrationDto);
            var result = await _repository.Authentication.RegisterUserAsync(admin, adminRegistrationDto.Password);
            var userReadDto = _mapper.Map<AppUserReadDto>(admin);


            if (result.IsSuccess)
            {
                var workstationAssignationResult = await _repository.Authentication.AddToWorkstationAsync(admin, workstation);

                if (workstationAssignationResult.IsSuccess)
                {
                    var userWorkstations = await _repository.Authentication.GetUsersWorkstationsAsync(admin);
                    //if (userWorkstations.Any()) userReadDto.Workstation = _mapper.Map<Workstation, WorkstationReadDto>(await _repository.Workstation.GetWorkstationByNameAsync(userWorkstations.First()));

                    return Ok(userReadDto);
                }
                else
                {
                    foreach (var error in workstationAssignationResult.ErrorDetails)
                    {
                        ModelState.AddModelError(error, error);
                    }
                    return ValidationProblem(ModelState);
                }
            }
            else
            {
                foreach (var error in result.ErrorDetails)
                {
                    ModelState.AddModelError(error, error);
                }
                return ValidationProblem(ModelState);
            }
        }

    }
}

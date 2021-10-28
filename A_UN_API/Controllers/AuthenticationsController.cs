﻿using AutoMapper;
using Contracts;
using Entities.DataTransfertObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace A_UN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationsController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly string _baseURL;



        public AuthenticationsController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _baseURL = string.Concat(httpContextAccessor.HttpContext.Request.Scheme, "://", httpContextAccessor.HttpContext.Request.Host);
        }




        //GET api/authentications/users/count
        [HttpGet("users/count")]
        [AllowAnonymous]
        public async Task<int> GetUsersCount()
        {
            _logger.LogInfo($"Count users of database.");
            return (await _repository.Authentication.CountUsersAsync());
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

                /*
                string url = $"{_baseURL}/api/authentication/confirmemail?userId={userReadDto.Id}&token={result.Token}";

                var email = new EmailData
                {
                    ToEmail = userReadDto.Email,
                    ToName = userReadDto.Firstname,
                    Subject = "Confirm your email",
                    Body = $"<h1>Welcome to A-UN</h1><p>Please confirm your email by <a href='{url}'>Clicking here<a/></p>",
                };

                await _repository.Mail.SendEmailAsync(email);
                */
                await SendVerificationEmail(userReadDto.Id);
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

                    //await SendVerificationEmail(userReadDto.Id);

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

                    if (authenticationResponseReadDto.UserInfo["ImgUrl"] != null) authenticationResponseReadDto.UserInfo["ImgUrl"] = $"{_baseURL}{authenticationResponseReadDto.UserInfo["ImgUrl"]}";

                    return Ok(authenticationResponseReadDto);
                }

                _logger.LogError($"Authentication failed : {authenticationResponse.Message}");
                return NotFound(authenticationResponse.Message);
            }

            return ValidationProblem(ModelState);
        }




        //POST api/authentications/login
        [HttpPost("resend-verification-email")]
        public async Task<ActionResult> ResendVerificationEmail()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (string.IsNullOrWhiteSpace(userId)) return BadRequest("userId or token invalid");

            await SendVerificationEmail(userId);

            return Ok("Verification email sent successfully");
        }




        private async Task SendVerificationEmail(string userId)
        {
            var user = await _repository.AppUser.GetAppUserByIdAsync(userId);
            var token = await _repository.Authentication.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = await _repository.Authentication.EncodeTokenAsync(token);

            string url = $"{_baseURL}/api/authentications/confirmemail?userId={userId}&token={encodedToken}";

            var email = new EmailData
            {
                ToEmail = user.Email,
                ToName = user.Firstname,
                Subject = "Confirm your email",
                Body = $"<h1>Welcome to A-UN</h1><p>Please confirm your email by <a href='{url}'>Clicking here</a></p>",
            };

            await _repository.Mail.SendEmailAsync(email);
        }




        //POST api/authentications/login
        [HttpGet("confirmemail")]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token)) return BadRequest("userId or token invalid");

            var result = await _repository.Authentication.ConfirmEmailAsync(userId, token);

            if (result.IsSuccess)
            {
                return Ok($"{result.UserInfo["Email"]} Email confirmed");
            }

            return BadRequest(result);
        }




        //POST api/authentications/forgetpassword
        [HttpPost("forgetpassword")]
        [AllowAnonymous]
        public async Task<ActionResult> ForgetPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return BadRequest("invalid email");

            var result = await _repository.Authentication.ForgetPasswordAsync(email);

            if (result.IsSuccess)
            {

                string url = $"{_baseURL}/api/authentications/resetpassword?email={email}&token={result.Token}";

                var emailData = new EmailData
                {
                    ToEmail = result.UserInfo["Email"],
                    ToName = result.UserInfo["Name"],
                    Subject = "Reset Password",
                    Body = $"<h1>Follow the instruction to reset your password</h1> <p> To reset your password <a href='{url}'>Clicking here</a></p>",
                };

                await _repository.Mail.SendEmailAsync(emailData);

                return Ok(result);
            }

            return BadRequest(result);
        }




        //POST api/authentications/forgetpassword
        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _repository.Authentication.ResetPasswordAsync(model);

                if (result.IsSuccess) return Ok(result);
            }

            return ValidationProblem(ModelState);
        }
    }
}

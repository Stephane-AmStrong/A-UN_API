using Contracts;
using Entities;
using Entities.Models;
using Entities.Models.QueryParameters;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class AuthenticationRepository : RepositoryBase<AppUser>, IAuthenticationRepository
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<Workstation> _roleManager;

        public AuthenticationRepository(RepositoryContext repositoryContext, UserManager<AppUser> userManager, RoleManager<Workstation> roleManager, IConfiguration configuration) : base(repositoryContext)
        {
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task<PagedList<AppUser>> GetAllUsersAsync(QueryStringParameters paginationParameters)
        {
            return await Task.Run(() =>
                        PagedList<AppUser>.ToPagedList(FindAll().OrderBy(x => x.Name),
                            paginationParameters.PageNumber,
                            paginationParameters.PageSize)
                        );
        }


        public async Task<AuthenticationResponse> LoginWithUserNameAsync(LoginRequest loginRequest, string password)
        {
            if (loginRequest == null) throw new ArgumentNullException(nameof(loginRequest));

            var user = await _userManager.FindByEmailAsync(loginRequest.Email);

            if (user == null)
            {
                return new AuthenticationResponse
                {
                    Message = "There is no user with that email adresse",
                    IsSuccess = false,
                };
            }

            var resultSucceeded = await _userManager.CheckPasswordAsync(user, password);

            if (!resultSucceeded)
            {
                return new AuthenticationResponse
                {
                    Message = "Invalid password !",
                    IsSuccess = false,
                };
            }
            
            var privateClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                //new Claim("FullName", user.Firstname+ " "+ user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                //new Claim("FullName", user.Firstname+ " "+ user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roleName = (await _userManager.GetRolesAsync(user))[0];
            var role =  await _roleManager.FindByNameAsync(roleName);

            claims.AddRange(await _roleManager.GetClaimsAsync(role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["AuthSettings:Issuer"],
                audience: _configuration["AuthSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            var userInfo = new Dictionary<string, string>
            {
                { "ImgUrl", user.ImgUrl },
                { "Name", user.Name},
                { "Email", user.Email },
                { "PhoneNumber", user.PhoneNumber },
                { "CreatedAt", user.CreatedAt.ToString()},
                { "UpdatedAt", user.UpdatedAt.ToString()},
                { "DisabledAt", user.DisabledAt.ToString()},
            };

            return new AuthenticationResponse
            {
                UserInfo = userInfo,
                AppUser = user,
                Token = tokenString,
                IsSuccess = true,
                ExpireDate = token.ValidTo,
            };
        }


        public async Task<AuthenticationResponse> RegisterUserAsync(AppUser user, string password)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.CreatedAt = DateTime.Now;
            user.UpdatedAt = DateTime.Now;

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = true,
                };
            }

            return new AuthenticationResponse
            {
                Message = "AppUser is not created",
                IsSuccess = false,
                ErrorDetails = result.Errors.Select(errorDescription => errorDescription.Description)
            };
        }


        public async Task<AuthenticationResponse> AddToWorkstationAsync(AppUser user, Workstation role)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (role == null) throw new ArgumentNullException(nameof(role));

            var result = await _userManager.AddToRoleAsync(user, role.Name);

            if (result.Succeeded)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = true,
                };
            }

            return new AuthenticationResponse
            {
                Message = "Workstation not assigned",
                IsSuccess = false,
                ErrorDetails = result.Errors.Select(errorDescription => errorDescription.Description)
            };
        }

        public async Task<AuthenticationResponse> RemoveFromWorkstationAsync(AppUser user, Workstation role)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (role == null) throw new ArgumentNullException(nameof(role));

            var result = await _userManager.RemoveFromRoleAsync(user, role.Name);

            if (result.Succeeded)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = true,
                };
            }

            return new AuthenticationResponse
            {
                Message = "Workstation not removed",
                IsSuccess = false,
                ErrorDetails = result.Errors.Select(errorDescription => errorDescription.Description)
            };
        }


        public async Task<ICollection<string>> GetUsersWorkstationsAsync(AppUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }


        public async Task<int> CountUsersAsync()
        {
            return await FindAll().CountAsync();
        }

        public async Task<string> GetUserId(ClaimsPrincipal user)
        {
            return await (Task.Run(() => _userManager.GetUserId(user)));
            //return await _userManager.GetUserIdAsync(user);
        }
    }
}

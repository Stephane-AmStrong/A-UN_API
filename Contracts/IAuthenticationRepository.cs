using Entities.Models;
using Entities.Models.QueryParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IAuthenticationRepository
    {
        Task<PagedList<AppUser>> GetAllUsersAsync(QueryStringParameters paginationParameters);
        Task<int> CountUsersAsync();
        Task<AuthenticationResponse> RegisterUserAsync(AppUser appUser, string password);
        Task<AuthenticationResponse> ConfirmEmailAsync(string userId, string token);
        Task<string> GenerateEmailConfirmationTokenAsync(AppUser appUser);
        //Task<string> GenerateEmailConfirmationTokenAsync(string userId);
        Task<string> EncodeTokenAsync(string token);
        Task<string> DecodeTokenAsync(string encodedToken);
        Task<AuthenticationResponse> LoginWithUserNameAsync(LoginRequest loginRequest, string password);
        Task<AuthenticationResponse> ForgetPasswordAsync(string email);
        Task<AuthenticationResponse> ResetPasswordAsync(ResetPasswordViewModel model);
        Task<string> GetUserId (ClaimsPrincipal user);

        Task<ICollection<string>> GetUsersWorkstationsAsync(AppUser user);
        Task<AuthenticationResponse> AddToWorkstationAsync(AppUser user, Workstation workstation);
        Task<AuthenticationResponse> RemoveFromWorkstationAsync(AppUser user, Workstation workstation);
    }
}

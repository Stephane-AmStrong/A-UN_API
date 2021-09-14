
using Entities.Models;
using Entities.Models.QueryParameters;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IAppUserRepository
    {
        Task<PagedList<AppUser>> GetAllAppUsersAsync(QueryStringParameters paginationParameters);

        Task<AppUser> GetAppUserByIdAsync(Guid id);
        Task<bool> AppUserExistAsync(AppUser appUser);

        Task UpdateAppUserAsync(AppUser appUser);
        Task DeleteAppUserAsync(AppUser appUser);
    }
}

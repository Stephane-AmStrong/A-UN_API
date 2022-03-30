
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IAppUserRepository /*: IRepositoryBase<AppUser>*/
    {
        Task<PagedList<AppUser>> GetAppUsersAsync(AppUserQueryParameters appUserParameters);

        Task<AppUser> GetAppUserByIdAsync(string id);
        Task<bool> AppUserExistAsync(AppUser appUser);
        Task<bool> IsInRoleAsync(AppUser appUser, string roneName);
        Task UpdateAppUserAsync(AppUser appUser);
        Task DeleteAppUserAsync(AppUser appUser);
    }
}

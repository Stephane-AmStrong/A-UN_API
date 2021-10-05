﻿
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IAppUserRepository
    {
        Task<PagedList<AppUser>> GetAllAppUsersAsync(QueryStringParameters paginationParameters);

        Task<AppUser> GetAppUserByIdAsync(string id);
        Task<bool> AppUserExistAsync(AppUser appUser);

        Task UpdateAppUserAsync(AppUser appUser);
        Task DeleteAppUserAsync(AppUser appUser);
    }
}

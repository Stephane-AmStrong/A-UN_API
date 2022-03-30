using Contracts;
using Entities;
using Entities.Helpers;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class AppUserRepository : RepositoryBase<AppUser>, IAppUserRepository
    {
        private ISortHelper<AppUser> _sortHelper;
        private readonly UserManager<AppUser> _userManager;

        public AppUserRepository(RepositoryContext repositoryContext, ISortHelper<AppUser> sortHelper, UserManager<AppUser> userManager) : base(repositoryContext)
        {
            _userManager = userManager;
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<AppUser>> GetAppUsersAsync(AppUserQueryParameters appUserParameters)
        {
            var appUsers = Enumerable.Empty<AppUser>().AsQueryable();

            ApplyFilters(ref appUsers, appUserParameters);

            PerformSearch(ref appUsers, appUserParameters.SearchTerm);

            var sortedAppUsers = _sortHelper.ApplySort(appUsers, appUserParameters.OrderBy);

            //foreach (var appUser in sortedAppUsers)
            //{
            //    var roleName = (await _userManager.GetRolesAsync(appUser))[0];
            //    appUser.Role = roleName;
            //}

            return await Task.Run(() =>
                PagedList<AppUser>.ToPagedList
                (
                    sortedAppUsers,
                    appUserParameters.PageNumber,
                    appUserParameters.PageSize)
                );
        }

        public async Task<AppUser> GetAppUserByIdAsync(string id)
        {
            return await _userManager.Users.Where(appUser => appUser.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsInRoleAsync(AppUser appUser, string roneName)
        {
            return await _userManager.IsInRoleAsync(appUser, roneName);
        }

        public async Task<bool> AppUserExistAsync(AppUser appUser)
        {
            return await _userManager.Users.Where(x => x.Name == appUser.Name)
                .AnyAsync();
        }

        public async Task UpdateAppUserAsync(AppUser appUser)
        {
            await _userManager.UpdateAsync(appUser);
        }

        public async Task DeleteAppUserAsync(AppUser appUser)
        {
            await _userManager.DeleteAsync(appUser);
        }

        #region ApplyFilters and PerformSearch Region

        private void ApplyFilters(ref IQueryable<AppUser> appUsers, AppUserQueryParameters appUserParameters)
        {
            appUsers = FindAll()
                .Include(x=> x.Payments)
                .Include(x => x.Subscriptions).ThenInclude(x => x.Formation).ThenInclude(x => x.University);

            //var userWorkstations = await Task.Run(() => _userManager.Users.Join(_repositoryContext.AppUserWorkstations, user => user.Id, role => role.UserId, (user, role) => role).Where(x => x.RoleId == id).OrderBy(x => x.User.Name).ThenBy(x => x.User.FirstName));

            //var userWorkstations = await _userManager.Users.Join(_repositoryContext.AppUserWorkstations, user => user.Id, role => role.UserId, (user, role) => role).Where(x => x.RoleId == id).Include(x=>x.User).OrderBy(x => x.User.Name).ThenBy(x => x.User.FirstName).ToListAsync();

            if (!string.IsNullOrEmpty(appUserParameters.WithRoleName))
            {
                var taskAppUsers = Task.Run(async () => await _userManager.GetUsersInRoleAsync(appUserParameters.WithRoleName));
                appUsers = taskAppUsers.Result.AsQueryable();
            }

            if (appUserParameters.OfFormationId != new Guid())
            {
                appUsers = appUsers.Where(x => x.Subscriptions.Any(x=>x.FormationId == appUserParameters.OfFormationId));
            }
            
            if (appUserParameters.OfFormationId != new Guid())
            {
                appUsers = appUsers.Where(x => x.Subscriptions.Any(x=>x.FormationId == appUserParameters.OfFormationId));
            }
            
            if (appUserParameters.FromUniversityId != new Guid())
            {
                appUsers = appUsers.Where(x => x.Subscriptions.Any(x=>x.Formation.UniversityId == appUserParameters.FromUniversityId));
            }
            
            if (!string.IsNullOrEmpty(appUserParameters.ManagedByAppUserId))
            {
                appUsers = appUsers.Where(x => x.Subscriptions.Any(x=>x.Formation.University.AppUserId == appUserParameters.ManagedByAppUserId));
            }
            
            if (appUserParameters.DisplayStudentOnly)
            {
                var taskAppUsers = Task.Run(async () => await _userManager.GetUsersInRoleAsync("Etudiant"));
                appUsers = taskAppUsers.Result.AsQueryable();
            }
            
            if (!string.IsNullOrEmpty(appUserParameters.WhitchPaidFrom))
            {
                var paidFrom = DateTime.Parse(appUserParameters.WhitchPaidFrom);
                appUsers = appUsers.Where(x => x.Payments.Any(x => x.PaidAt >= paidFrom));
            }

            if (!string.IsNullOrEmpty(appUserParameters.WhitchPaidTo))
            {
                var paidTo = DateTime.Parse(appUserParameters.WhitchPaidTo);
                appUsers = appUsers.Where(x => x.Payments.Any(x => x.PaidAt <= paidTo));
            }
        }

        private void PerformSearch(ref IQueryable<AppUser> appUsers, string searchTerm)
        {
            if (!appUsers.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            appUsers = appUsers.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion
    }
}

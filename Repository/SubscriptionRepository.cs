using Contracts;
using Entities;
using Entities.Extensions;
using Entities.Helpers;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class SubscriptionRepository : RepositoryBase<Subscription>, ISubscriptionRepository
    {
        private ISortHelper<Subscription> _sortHelper;
        private IDataShaper<Subscription> _dataShaper;

        public SubscriptionRepository(
            RepositoryContext repositoryContext, 
            ISortHelper<Subscription> sortHelper,
            IDataShaper<Subscription> dataShaper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }

        public async Task<PagedList<Entity>> GetSubscriptionsAsync(SubscriptionParameters subscriptionParameters)
        {
            var subscriptions = Enumerable.Empty<Subscription>().AsQueryable();

            ApplyFilters(ref subscriptions, subscriptionParameters);

            PerformSearch(ref subscriptions, subscriptionParameters.SearchTerm);

            var sortedSubscriptions = _sortHelper.ApplySort(subscriptions, subscriptionParameters.OrderBy);
            var shapedSubscriptions = _dataShaper.ShapeData(sortedSubscriptions, subscriptionParameters.Fields);

            return await Task.Run(() =>
                PagedList<Entity>.ToPagedList
                (
                    shapedSubscriptions,
                    subscriptionParameters.PageNumber,
                    subscriptionParameters.PageSize)
                );
        }

        public async Task<Entity> GetSubscriptionByIdAsync(Guid id, string fields)
        {
            var subscription = FindByCondition(subscription => subscription.Id.Equals(id))
                .DefaultIfEmpty(new Subscription())
                .FirstOrDefault();

            return await Task.Run(() =>
                _dataShaper.ShapeData(subscription, fields)
            );
        }

        public async Task<Subscription> GetSubscriptionByIdAsync(Guid id)
        {
            return await FindByCondition(subscription => subscription.Id.Equals(id))
                .Include(x => x.AcademicYear).Include(x => x.AppUser).Include(x => x.FormationLevel).ThenInclude(x => x.Formation).ThenInclude(x => x.University)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> SubscriptionExistAsync(Subscription subscription)
        {
            return await FindByCondition(x => x.AcademicYearId == subscription.AcademicYearId && x.AppUserId == subscription.AppUserId && x.FormationLevelId == subscription.FormationLevelId)
                .AnyAsync();
        }

        public async Task CreateSubscriptionAsync(Subscription subscription)
        {
            subscription.CreatedAt = DateTime.Now;
            subscription.UpdatedAt = DateTime.Now;
            await CreateAsync(subscription);
        }

        public async Task UpdateSubscriptionAsync(Subscription subscription)
        {
            subscription.UpdatedAt = DateTime.Now;
            await UpdateAsync(subscription);
        }

        public async Task DeleteSubscriptionAsync(Subscription subscription)
        {
            await DeleteAsync(subscription);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<Subscription> subscriptions, SubscriptionParameters subscriptionParameters)
        {
            subscriptions = FindAll()
                .Include(x=>x.AcademicYear).Include(x=>x.AppUser).Include(x=>x.FormationLevel).ThenInclude(x=>x.Formation).ThenInclude(x=>x.University);
            if (!string.IsNullOrWhiteSpace(subscriptionParameters.AppUserId))
            {
                subscriptions = subscriptions.Where(x => x.AppUserId == subscriptionParameters.AppUserId);
            }

            if (subscriptionParameters.validateOnly)
            {
                subscriptions = subscriptions.Where(x => x.ValiddatedAt != null);
            }

            /*
            if (subscriptionParameters.MaxBirthday != null)
            {
                subscriptions = subscriptions.Where(x => x.Birthday < subscriptionParameters.MaxBirthday);
            }

            if (subscriptionParameters.MinCreateAt != null)
            {
                subscriptions = subscriptions.Where(x => x.CreateAt >= subscriptionParameters.MinCreateAt);
            }

            if (subscriptionParameters.MaxCreateAt != null)
            {
                subscriptions = subscriptions.Where(x => x.CreateAt < subscriptionParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<Subscription> subscriptions, string searchTerm)
        {
            if (!subscriptions.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            subscriptions = subscriptions.Where(x => x.AppUser.Name.ToLower().Contains(searchTerm.Trim().ToLower()) || x.FormationLevel.Name.ToLower().Contains(searchTerm.Trim().ToLower()) || x.FormationLevel.Formation.Name.ToLower().Contains(searchTerm.Trim().ToLower()) || x.FormationLevel.Formation.University.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}

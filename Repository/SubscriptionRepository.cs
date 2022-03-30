using Contracts;
using Entities;
using Entities.Helpers;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class SubscriptionRepository : RepositoryBase<Subscription>, ISubscriptionRepository
    {
        private ISortHelper<Subscription> _sortHelper;

        public SubscriptionRepository(
            RepositoryContext repositoryContext, 
            ISortHelper<Subscription> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<Subscription>> GetSubscriptionsAsync(SubscriptionQueryParameters subscriptionParameters)
        {
            var subscriptions = Enumerable.Empty<Subscription>().AsQueryable();

            ApplyFilters(ref subscriptions, subscriptionParameters);

            PerformSearch(ref subscriptions, subscriptionParameters.SearchTerm);

            var sortedSubscriptions = _sortHelper.ApplySort(subscriptions, subscriptionParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<Subscription>.ToPagedList
                (
                    sortedSubscriptions,
                    subscriptionParameters.PageNumber,
                    subscriptionParameters.PageSize)
                );
        }

        public async Task<Subscription> GetSubscriptionByIdAsync(Guid id)
        {
            return await FindByCondition(subscription => subscription.Id.Equals(id))
                .Include(x => x.AcademicYear).Include(x => x.AppUser).Include(x => x.Formation).ThenInclude(x => x.University)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> SubscriptionExistAsync(Subscription subscription)
        {
            return await FindByCondition(x => x.AcademicYearId == subscription.AcademicYearId && x.AppUserId == subscription.AppUserId && x.FormationId == subscription.FormationId)
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
        private void ApplyFilters(ref IQueryable<Subscription> subscriptions, SubscriptionQueryParameters subscriptionParameters)
        {
            subscriptions = FindAll()
                .Include(x=>x.AcademicYear).Include(x=>x.AppUser).Include(x=>x.Formation).ThenInclude(x=>x.University);
            if (!string.IsNullOrWhiteSpace(subscriptionParameters.OfAppUserId))
            {
                subscriptions = subscriptions.Where(x => x.AppUserId == subscriptionParameters.OfAppUserId);
            }

            if (subscriptionParameters.validateOnly)
            {
                subscriptions = subscriptions.Where(x => x.ValidatedAt != null);
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

            subscriptions = subscriptions.Where(x => x.AppUser.Name.ToLower().Contains(searchTerm.Trim().ToLower()) || x.Formation.Name.ToLower().Contains(searchTerm.Trim().ToLower()) || x.Formation.Name.ToLower().Contains(searchTerm.Trim().ToLower()) || x.Formation.University.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}

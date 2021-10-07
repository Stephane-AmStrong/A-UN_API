using Contracts;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class SubscriptionRepository : RepositoryBase<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<PagedList<Subscription>> GetAllSubscriptionsAsync(QueryStringParameters paginationParameters)
        {
            return await Task.Run(() =>
                PagedList<Subscription>.ToPagedList(FindAll(),
                    paginationParameters.PageNumber,
                    paginationParameters.PageSize)
                );
        }

        public async Task<Subscription> GetSubscriptionByIdAsync(Guid id)
        {
            return await FindByCondition(subscription => subscription.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> SubscriptionExistAsync(Entities.Models.Subscription subscription)
        {
            return await FindByCondition(x => x.Name == subscription.Name)
                .AnyAsync();
        }

        public async Task CreateSubscriptionAsync(Entities.Models.Subscription subscription)
        {
            await CreateAsync(subscription);
        }

        public async Task UpdateSubscriptionAsync(Entities.Models.Subscription subscription)
        {
            await UpdateAsync(subscription);
        }

        public async Task DeleteSubscriptionAsync(Entities.Models.Subscription subscription)
        {
            await DeleteAsync(subscription);
        }
    }
}

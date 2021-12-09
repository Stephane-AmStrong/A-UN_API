
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ISubscriptionRepository
    {
        Task<PagedList<Entity>> GetSubscriptionsAsync(SubscriptionParameters subscriptionParameters);

        Task<Subscription> GetSubscriptionByIdAsync(Guid id);
        Task<bool> SubscriptionExistAsync(Subscription subscription);

        Task CreateSubscriptionAsync(Subscription subscription);
        Task UpdateSubscriptionAsync(Subscription subscription);
        Task DeleteSubscriptionAsync(Subscription subscription);
    }
}

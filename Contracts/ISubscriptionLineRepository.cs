
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ISubscriptionLineRepository
    {
        Task<PagedList<Entity>> GetSubscriptionLinesAsync(SubscriptionLineQueryParameters subscriptionLineParameters);

        Task<SubscriptionLine> GetSubscriptionLineByIdAsync(Guid id);
        Task<bool> SubscriptionLineExistAsync(SubscriptionLine subscriptionLine);

        Task CreateSubscriptionLineAsync(SubscriptionLine subscriptionLine);
        Task UpdateSubscriptionLineAsync(SubscriptionLine subscriptionLine);
        Task DeleteSubscriptionLineAsync(SubscriptionLine subscriptionLine);
    }
}

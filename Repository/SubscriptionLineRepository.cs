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
    public class SubscriptionLineRepository : RepositoryBase<SubscriptionLine>, ISubscriptionLineRepository
    {
        public SubscriptionLineRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<PagedList<SubscriptionLine>> GetAllSubscriptionLinesAsync(QueryStringParameters paginationParameters)
        {
            return await Task.Run(() =>
                PagedList<SubscriptionLine>.ToPagedList(FindAll(),
                    paginationParameters.PageNumber,
                    paginationParameters.PageSize)
                );
        }

        public async Task<SubscriptionLine> GetSubscriptionLineByIdAsync(Guid id)
        {
            return await FindByCondition(subscriptionLine => subscriptionLine.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> SubscriptionLineExistAsync(Entities.Models.SubscriptionLine subscriptionLine)
        {
            return await FindByCondition(x => x.Name == subscriptionLine.Name)
                .AnyAsync();
        }

        public async Task CreateSubscriptionLineAsync(Entities.Models.SubscriptionLine subscriptionLine)
        {
            await CreateAsync(subscriptionLine);
        }

        public async Task UpdateSubscriptionLineAsync(Entities.Models.SubscriptionLine subscriptionLine)
        {
            await UpdateAsync(subscriptionLine);
        }

        public async Task DeleteSubscriptionLineAsync(Entities.Models.SubscriptionLine subscriptionLine)
        {
            await DeleteAsync(subscriptionLine);
        }
    }
}

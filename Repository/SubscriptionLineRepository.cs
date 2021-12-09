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
    public class SubscriptionLineRepository : RepositoryBase<SubscriptionLine>, ISubscriptionLineRepository
    {
        private ISortHelper<SubscriptionLine> _sortHelper;
        private IDataShaper<SubscriptionLine> _dataShaper;

        public SubscriptionLineRepository(
            RepositoryContext repositoryContext, 
            ISortHelper<SubscriptionLine> sortHelper,
            IDataShaper<SubscriptionLine> dataShaper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }

        public async Task<PagedList<Entity>> GetSubscriptionLinesAsync(SubscriptionLineParameters subscriptionLineParameters)
        {
            var subscriptionLines = Enumerable.Empty<SubscriptionLine>().AsQueryable();

            ApplyFilters(ref subscriptionLines, subscriptionLineParameters);

            PerformSearch(ref subscriptionLines, subscriptionLineParameters.SearchTerm);

            var sortedSubscriptionLines = _sortHelper.ApplySort(subscriptionLines, subscriptionLineParameters.OrderBy);
            var shapedSubscriptionLines = _dataShaper.ShapeData(sortedSubscriptionLines, subscriptionLineParameters.Fields);

            return await Task.Run(() =>
                PagedList<Entity>.ToPagedList
                (
                    shapedSubscriptionLines,
                    subscriptionLineParameters.PageNumber,
                    subscriptionLineParameters.PageSize)
                );
        }

        public async Task<Entity> GetSubscriptionLineByIdAsync(Guid id, string fields)
        {
            var subscriptionLine = FindByCondition(subscriptionLine => subscriptionLine.Id.Equals(id))
                .DefaultIfEmpty(new SubscriptionLine())
                .FirstOrDefault();

            return await Task.Run(() =>
                _dataShaper.ShapeData(subscriptionLine, fields)
            );
        }

        public async Task<SubscriptionLine> GetSubscriptionLineByIdAsync(Guid id)
        {
            return await FindByCondition(subscriptionLine => subscriptionLine.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> SubscriptionLineExistAsync(SubscriptionLine subscriptionLine)
        {
            return await FindByCondition(x => x.Name == subscriptionLine.Name)
                .AnyAsync();
        }

        public async Task CreateSubscriptionLineAsync(SubscriptionLine subscriptionLine)
        {
            await CreateAsync(subscriptionLine);
        }

        public async Task UpdateSubscriptionLineAsync(SubscriptionLine subscriptionLine)
        {
            await UpdateAsync(subscriptionLine);
        }

        public async Task DeleteSubscriptionLineAsync(SubscriptionLine subscriptionLine)
        {
            await DeleteAsync(subscriptionLine);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<SubscriptionLine> subscriptionLines, SubscriptionLineParameters subscriptionLineParameters)
        {
            subscriptionLines = FindAll();
            /*
            if (!string.IsNullOrWhiteSpace(subscriptionLineParameters.AppUserId))
            {
                subscriptionLines = subscriptionLines.Where(x => x.AppUserId == subscriptionLineParameters.AppUserId);
            }

            if (subscriptionLineParameters.MinBirthday != null)
            {
                subscriptionLines = subscriptionLines.Where(x => x.Birthday >= subscriptionLineParameters.MinBirthday);
            }

            if (subscriptionLineParameters.MaxBirthday != null)
            {
                subscriptionLines = subscriptionLines.Where(x => x.Birthday < subscriptionLineParameters.MaxBirthday);
            }

            if (subscriptionLineParameters.MinCreateAt != null)
            {
                subscriptionLines = subscriptionLines.Where(x => x.CreateAt >= subscriptionLineParameters.MinCreateAt);
            }

            if (subscriptionLineParameters.MaxCreateAt != null)
            {
                subscriptionLines = subscriptionLines.Where(x => x.CreateAt < subscriptionLineParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<SubscriptionLine> subscriptionLines, string searchTerm)
        {
            if (!subscriptionLines.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            subscriptionLines = subscriptionLines.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}

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
    public class AboutRepository : RepositoryBase<About>, IAboutRepository
    {
        private ISortHelper<About> _sortHelper;

        public AboutRepository(
            RepositoryContext repositoryContext,
            ISortHelper<About> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<About>> GetAboutsAsync(AboutQueryParameters aboutParameters)
        {
            var abouts = Enumerable.Empty<About>().AsQueryable();

            ApplyFilters(ref abouts, aboutParameters);

            PerformSearch(ref abouts, aboutParameters.SearchTerm);

            var sortedAbouts = _sortHelper.ApplySort(abouts, aboutParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<About>.ToPagedList
                (
                    sortedAbouts,
                    aboutParameters.PageNumber,
                    aboutParameters.PageSize)
                );
        }

        public async Task<About> GetAboutByIdAsync(Guid id)
        {
            return await FindByCondition(about => about.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> AboutExistAsync(About about)
        {
            return await FindByCondition(x => x.Content == about.Content && x.ImgLink == about.ImgLink)
                .AnyAsync();
        }

        public async Task CreateAboutAsync(About about)
        {
            await CreateAsync(about);
        }

        public async Task UpdateAboutAsync(About about)
        {
            await UpdateAsync(about);
        }

        public async Task DeleteAboutAsync(About about)
        {
            await DeleteAsync(about);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<About> abouts, AboutQueryParameters aboutParameters)
        {
            abouts = FindAll();
        }

        private void PerformSearch(ref IQueryable<About> abouts, string searchTerm)
        {
            if (!abouts.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            abouts = abouts.Where(x => x.Content.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}

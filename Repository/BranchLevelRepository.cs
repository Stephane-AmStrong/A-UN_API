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
    public class BranchLevelRepository : RepositoryBase<BranchLevel>, IBranchLevelRepository
    {
        private ISortHelper<BranchLevel> _sortHelper;
        private IDataShaper<BranchLevel> _dataShaper;

        public BranchLevelRepository(
            RepositoryContext repositoryContext, 
            ISortHelper<BranchLevel> sortHelper,
            IDataShaper<BranchLevel> dataShaper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }

        public async Task<PagedList<Entity>> GetAllBranchLevelsAsync(BranchLevelParameters branchLevelParameters)
        {
            var branchLevels = Enumerable.Empty<BranchLevel>().AsQueryable();

            ApplyFilters(ref branchLevels, branchLevelParameters);

            PerformSearch(ref branchLevels, branchLevelParameters.SearchTerm);

            var sortedBranchLevels = _sortHelper.ApplySort(branchLevels, branchLevelParameters.OrderBy);
            var shapedBranchLevels = _dataShaper.ShapeData(sortedBranchLevels, branchLevelParameters.Fields);

            return await Task.Run(() =>
                PagedList<Entity>.ToPagedList
                (
                    shapedBranchLevels,
                    branchLevelParameters.PageNumber,
                    branchLevelParameters.PageSize)
                );
        }

        public async Task<Entity> GetBranchLevelByIdAsync(Guid id, string fields)
        {
            var branchLevel = FindByCondition(branchLevel => branchLevel.Id.Equals(id))
                .DefaultIfEmpty(new BranchLevel())
                .FirstOrDefault();

            return await Task.Run(() =>
                _dataShaper.ShapeData(branchLevel, fields)
            );
        }

        public async Task<BranchLevel> GetBranchLevelByIdAsync(Guid id)
        {
            return await FindByCondition(branchLevel => branchLevel.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> BranchLevelExistAsync(BranchLevel branchLevel)
        {
            return await FindByCondition(x => x.Name == branchLevel.Name)
                .AnyAsync();
        }

        public async Task CreateBranchLevelAsync(BranchLevel branchLevel)
        {
            await CreateAsync(branchLevel);
        }

        public async Task UpdateBranchLevelAsync(BranchLevel branchLevel)
        {
            await UpdateAsync(branchLevel);
        }

        public async Task DeleteBranchLevelAsync(BranchLevel branchLevel)
        {
            await DeleteAsync(branchLevel);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<BranchLevel> branchLevels, BranchLevelParameters branchLevelParameters)
        {
            branchLevels = FindAll();
            /*
            if (!string.IsNullOrWhiteSpace(branchLevelParameters.AppUserId))
            {
                branchLevels = branchLevels.Where(x => x.AppUserId == branchLevelParameters.AppUserId);
            }

            if (branchLevelParameters.MinBirthday != null)
            {
                branchLevels = branchLevels.Where(x => x.Birthday >= branchLevelParameters.MinBirthday);
            }

            if (branchLevelParameters.MaxBirthday != null)
            {
                branchLevels = branchLevels.Where(x => x.Birthday < branchLevelParameters.MaxBirthday);
            }

            if (branchLevelParameters.MinCreateAt != null)
            {
                branchLevels = branchLevels.Where(x => x.CreateAt >= branchLevelParameters.MinCreateAt);
            }

            if (branchLevelParameters.MaxCreateAt != null)
            {
                branchLevels = branchLevels.Where(x => x.CreateAt < branchLevelParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<BranchLevel> branchLevels, string searchTerm)
        {
            if (!branchLevels.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            branchLevels = branchLevels.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}

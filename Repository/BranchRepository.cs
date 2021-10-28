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
    public class BranchRepository : RepositoryBase<Branch>, IBranchRepository
    {
        private ISortHelper<Branch> _sortHelper;
        private IDataShaper<Branch> _dataShaper;

        public BranchRepository(
            RepositoryContext repositoryContext, 
            ISortHelper<Branch> sortHelper,
            IDataShaper<Branch> dataShaper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }

        public async Task<PagedList<Entity>> GetAllBranchesAsync(BranchParameters branchParameters)
        {
            var branches = Enumerable.Empty<Branch>().AsQueryable();

            ApplyFilters(ref branches, branchParameters);

            PerformSearch(ref branches, branchParameters.SearchTerm);

            var sortedBranches = _sortHelper.ApplySort(branches, branchParameters.OrderBy);
            var shapedBranches = _dataShaper.ShapeData(sortedBranches, branchParameters.Fields);

            return await Task.Run(() =>
                PagedList<Entity>.ToPagedList
                (
                    shapedBranches,
                    branchParameters.PageNumber,
                    branchParameters.PageSize)
                );
        }

        public async Task<Entity> GetBranchByIdAsync(Guid id, string fields)
        {
            var branch = FindByCondition(branch => branch.Id.Equals(id))
                .DefaultIfEmpty(new Branch())
                .FirstOrDefault();

            return await Task.Run(() =>
                _dataShaper.ShapeData(branch, fields)
            );
        }

        public async Task<Branch> GetBranchByIdAsync(Guid id)
        {
            return await FindByCondition(branch => branch.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> BranchExistAsync(Branch branch)
        {
            return await FindByCondition(x => x.Name == branch.Name)
                .AnyAsync();
        }

        public async Task CreateBranchAsync(Branch branch)
        {
            await CreateAsync(branch);
        }

        public async Task UpdateBranchAsync(Branch branch)
        {
            await UpdateAsync(branch);
        }

        public async Task DeleteBranchAsync(Branch branch)
        {
            await DeleteAsync(branch);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<Branch> branches, BranchParameters branchParameters)
        {
            branches = FindAll();
            /*
            if (!string.IsNullOrWhiteSpace(branchParameters.AppUserId))
            {
                branches = branches.Where(x => x.AppUserId == branchParameters.AppUserId);
            }

            if (branchParameters.MinBirthday != null)
            {
                branches = branches.Where(x => x.Birthday >= branchParameters.MinBirthday);
            }

            if (branchParameters.MaxBirthday != null)
            {
                branches = branches.Where(x => x.Birthday < branchParameters.MaxBirthday);
            }

            if (branchParameters.MinCreateAt != null)
            {
                branches = branches.Where(x => x.CreateAt >= branchParameters.MinCreateAt);
            }

            if (branchParameters.MaxCreateAt != null)
            {
                branches = branches.Where(x => x.CreateAt < branchParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<Branch> branches, string searchTerm)
        {
            if (!branches.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            branches = branches.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}

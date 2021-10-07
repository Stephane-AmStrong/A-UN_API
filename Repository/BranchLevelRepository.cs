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
    public class BranchLevelRepository : RepositoryBase<BranchLevel>, IBranchLevelRepository
    {
        public BranchLevelRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<PagedList<BranchLevel>> GetAllBranchLevelsAsync(QueryStringParameters paginationParameters)
        {
            return await Task.Run(() =>
                PagedList<BranchLevel>.ToPagedList(FindAll(),
                    paginationParameters.PageNumber,
                    paginationParameters.PageSize)
                );
        }

        public async Task<BranchLevel> GetBranchLevelByIdAsync(Guid id)
        {
            return await FindByCondition(branchLevel => branchLevel.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> BranchLevelExistAsync(Entities.Models.BranchLevel branchLevel)
        {
            return await FindByCondition(x => x.Name == branchLevel.Name)
                .AnyAsync();
        }

        public async Task CreateBranchLevelAsync(Entities.Models.BranchLevel branchLevel)
        {
            await CreateAsync(branchLevel);
        }

        public async Task UpdateBranchLevelAsync(Entities.Models.BranchLevel branchLevel)
        {
            await UpdateAsync(branchLevel);
        }

        public async Task DeleteBranchLevelAsync(Entities.Models.BranchLevel branchLevel)
        {
            await DeleteAsync(branchLevel);
        }
    }
}

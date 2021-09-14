using Contracts;
using Entities;
using Entities.Models;
using Entities.Models.QueryParameters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class BranchRepository : RepositoryBase<Branch>, IBranchRepository
    {
        public BranchRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<PagedList<Branch>> GetAllBranchesAsync(QueryStringParameters paginationParameters)
        {
            return await Task.Run(() =>
                PagedList<Branch>.ToPagedList(FindAll().OrderBy(x => x.Name),
                    paginationParameters.PageNumber,
                    paginationParameters.PageSize)
                );
        }

        public async Task<Branch> GetBranchByIdAsync(Guid id)
        {
            return await FindByCondition(branch => branch.Id.Equals(id))
                
                .OrderBy(x => x.Name)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> BranchExistAsync(Entities.Models.Branch branch)
        {
            return await FindByCondition(x => x.Name == branch.Name)
                .AnyAsync();
        }

        public async Task CreateBranchAsync(Entities.Models.Branch branch)
        {
            await CreateAsync(branch);
        }

        public async Task UpdateBranchAsync(Entities.Models.Branch branch)
        {
            await UpdateAsync(branch);
        }

        public async Task DeleteBranchAsync(Entities.Models.Branch branch)
        {
            await DeleteAsync(branch);
        }
    }
}

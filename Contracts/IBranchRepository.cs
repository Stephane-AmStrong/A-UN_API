﻿
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IBranchRepository
    {
        Task<PagedList<Entity>> GetAllBranchesAsync(BranchParameters branchParameters);

        Task<Branch> GetBranchByIdAsync(Guid id);
        Task<bool> BranchExistAsync(Branch branch);

        Task CreateBranchAsync(Branch branch);
        Task UpdateBranchAsync(Branch branch);
        Task DeleteBranchAsync(Branch branch);
    }
}

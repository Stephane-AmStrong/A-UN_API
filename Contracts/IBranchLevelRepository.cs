﻿
using Entities.Models;
using Entities.Models.QueryParameters;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IBranchLevelRepository
    {
        Task<PagedList<BranchLevel>> GetAllBranchLevelsAsync(QueryStringParameters paginationParameters);

        Task<BranchLevel> GetBranchLevelByIdAsync(Guid id);
        Task<bool> BranchLevelExistAsync(BranchLevel branchLevel);

        Task CreateBranchLevelAsync(BranchLevel branchLevel);
        Task UpdateBranchLevelAsync(BranchLevel branchLevel);
        Task DeleteBranchLevelAsync(BranchLevel branchLevel);
    }
}
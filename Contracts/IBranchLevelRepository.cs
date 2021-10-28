
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IBranchLevelRepository
    {
        Task<PagedList<Entity>> GetAllBranchLevelsAsync(BranchLevelParameters branchlevelParameters);

        Task<BranchLevel> GetBranchLevelByIdAsync(Guid id);
        Task<bool> BranchLevelExistAsync(BranchLevel branchLevel);

        Task CreateBranchLevelAsync(BranchLevel branchLevel);
        Task UpdateBranchLevelAsync(BranchLevel branchLevel);
        Task DeleteBranchLevelAsync(BranchLevel branchLevel);
    }
}

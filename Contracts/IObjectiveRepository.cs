
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IObjectiveRepository
    {
        Task<PagedList<Objective>> GetAllObjectivesAsync(QueryStringParameters paginationParameters);

        Task<Objective> GetObjectiveByIdAsync(Guid id);
        Task<bool> ObjectiveExistAsync(Objective objective);

        Task CreateObjectiveAsync(Objective objective);
        Task UpdateObjectiveAsync(Objective objective);
        Task DeleteObjectiveAsync(Objective objective);
    }
}

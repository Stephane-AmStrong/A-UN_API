
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IFormationLevelRepository
    {
        Task<PagedList<Entity>> GetFormationLevelsAsync(FormationLevelParameters formationlevelParameters);

        Task<FormationLevel> GetFormationLevelByIdAsync(Guid id);
        Task<bool> FormationLevelExistAsync(FormationLevel formationLevel);

        Task CreateFormationLevelAsync(FormationLevel formationLevel);
        Task UpdateFormationLevelAsync(FormationLevel formationLevel);
        Task DeleteFormationLevelAsync(FormationLevel formationLevel);
    }
}

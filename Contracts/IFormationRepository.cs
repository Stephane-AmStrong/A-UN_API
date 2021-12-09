
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IFormationRepository
    {
        Task<PagedList<Entity>> GetFormationsAsync(FormationParameters formationParameters);

        Task<Formation> GetFormationByIdAsync(Guid id);
        Task<bool> FormationExistAsync(Formation formation);

        Task CreateFormationAsync(Formation formation);
        Task UpdateFormationAsync(Formation formation);
        Task DeleteFormationAsync(Formation formation);
    }
}


using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IPrerequisiteRepository
    {
        Task<PagedList<Entity>> GetPrerequisitesAsync(PrerequisiteQueryParameters registrationFormLineParameters);

        Task<int> GetNextNumberAsync(PrerequisiteQueryParameters registrationFormLineParameters);
        Task<Prerequisite> GetPrerequisiteByIdAsync(Guid id);
        Task<bool> PrerequisiteExistAsync(Prerequisite registrationFormLine);

        Task CreatePrerequisiteAsync(Prerequisite registrationFormLine);
        Task UpdatePrerequisiteAsync(Prerequisite registrationFormLine);
        Task DeletePrerequisiteAsync(Prerequisite registrationFormLine);
    }
}

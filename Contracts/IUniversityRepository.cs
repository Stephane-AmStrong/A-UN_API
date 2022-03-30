
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IUniversityRepository
    {
        Task<PagedList<University>> GetUniversitiesAsync(UniversityQueryParameters universityParameters);

        Task<University> GetUniversityByIdAsync(Guid id);
        Task<bool> UniversityExistAsync(University university);

        Task CreateUniversityAsync(University university);
        Task UpdateUniversityAsync(University university);
        Task DeleteUniversityAsync(University university);
    }
}

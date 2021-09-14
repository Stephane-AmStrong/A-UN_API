
using Entities.Models;
using Entities.Models.QueryParameters;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IUniversityRepository
    {
        Task<PagedList<University>> GetAllUniversitiesAsync(QueryStringParameters paginationParameters);

        Task<University> GetUniversityByIdAsync(Guid id);
        Task<bool> UniversityExistAsync(University university);

        Task CreateUniversityAsync(University university);
        Task UpdateUniversityAsync(University university);
        Task DeleteUniversityAsync(University university);
    }
}

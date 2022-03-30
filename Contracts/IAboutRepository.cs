
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IAboutRepository
    {
        Task<PagedList<About>> GetAboutsAsync(AboutQueryParameters aboutParameters);

        Task<About> GetAboutByIdAsync(Guid id);
        Task<bool> AboutExistAsync(About about);

        Task CreateAboutAsync(About about);
        Task UpdateAboutAsync(About about);
        Task DeleteAboutAsync(About about);
    }
}

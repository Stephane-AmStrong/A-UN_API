
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IPersonalFileRepository
    {
        Task<PagedList<Entity>> GetPersonalFilesAsync(PersonalFileQueryParameters personalfileParameters);

        Task<PersonalFile> GetPersonalFileByIdAsync(Guid id);
        Task<bool> PersonalFileExistAsync(PersonalFile personalFile);

        Task CreatePersonalFileAsync(PersonalFile personalFile);
        Task UpdatePersonalFileAsync(PersonalFile personalFile);
        Task DeletePersonalFileAsync(PersonalFile personalFile);
    }
}


using Entities.Models;
using Entities.Models.QueryParameters;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IPersonalFileRepository
    {
        Task<PagedList<PersonalFile>> GetAllPersonalFilesAsync(QueryStringParameters paginationParameters);

        Task<PersonalFile> GetPersonalFileByIdAsync(Guid id);
        Task<bool> PersonalFileExistAsync(PersonalFile personalFile);

        Task CreatePersonalFileAsync(PersonalFile personalFile);
        Task UpdatePersonalFileAsync(PersonalFile personalFile);
        Task DeletePersonalFileAsync(PersonalFile personalFile);
    }
}

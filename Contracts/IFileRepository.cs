
using Entities.Models;
using Entities.Models.QueryParameters;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IFileRepository
    {
        Task<PagedList<File>> GetAllFilesAsync(QueryStringParameters paginationParameters);

        Task<File> GetFileByIdAsync(Guid id);
        Task<bool> FileExistAsync(File file);

        Task CreateFileAsync(File file);
        Task UpdateFileAsync(File file);
        Task DeleteFileAsync(File file);
    }
}

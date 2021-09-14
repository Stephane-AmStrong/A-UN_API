using Contracts;
using Entities;
using Entities.Models;
using Entities.Models.QueryParameters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class FileRepository : RepositoryBase<File>, IFileRepository
    {
        public FileRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<PagedList<File>> GetAllFilesAsync(QueryStringParameters paginationParameters)
        {
            return await Task.Run(() =>
                PagedList<File>.ToPagedList(FindAll().OrderBy(x => x.Name),
                    paginationParameters.PageNumber,
                    paginationParameters.PageSize)
                );
        }

        public async Task<File> GetFileByIdAsync(Guid id)
        {
            return await FindByCondition(file => file.Id.Equals(id))
                
                .OrderBy(x => x.Name)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> FileExistAsync(Entities.Models.File file)
        {
            return await FindByCondition(x => x.Name == file.Name)
                .AnyAsync();
        }

        public async Task CreateFileAsync(Entities.Models.File file)
        {
            await CreateAsync(file);
        }

        public async Task UpdateFileAsync(Entities.Models.File file)
        {
            await UpdateAsync(file);
        }

        public async Task DeleteFileAsync(Entities.Models.File file)
        {
            await DeleteAsync(file);
        }
    }
}

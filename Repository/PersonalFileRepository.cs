using Contracts;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class PersonalFileRepository : RepositoryBase<PersonalFile>, IPersonalFileRepository
    {
        public PersonalFileRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<PagedList<PersonalFile>> GetAllPersonalFilesAsync(QueryStringParameters paginationParameters)
        {
            return await Task.Run(() =>
                PagedList<PersonalFile>.ToPagedList(FindAll(),
                    paginationParameters.PageNumber,
                    paginationParameters.PageSize)
                );
        }

        public async Task<PersonalFile> GetPersonalFileByIdAsync(Guid id)
        {
            return await FindByCondition(personalFile => personalFile.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> PersonalFileExistAsync(PersonalFile personalFile)
        {
            return await FindByCondition(x => x.Name == personalFile.Name)
                .AnyAsync();
        }

        public async Task CreatePersonalFileAsync(PersonalFile personalFile)
        {
            await CreateAsync(personalFile);
        }

        public async Task UpdatePersonalFileAsync(PersonalFile personalFile)
        {
            await UpdateAsync(personalFile);
        }

        public async Task DeletePersonalFileAsync(PersonalFile personalFile)
        {
            await DeleteAsync(personalFile);
        }
    }
}

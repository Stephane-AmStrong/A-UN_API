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
    public class UniversityRepository : RepositoryBase<University>, IUniversityRepository
    {
        public UniversityRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<PagedList<University>> GetAllUniversitiesAsync(QueryStringParameters paginationParameters)
        {
            return await Task.Run(() =>
                PagedList<University>.ToPagedList(FindAll().OrderBy(x => x.Name),
                    paginationParameters.PageNumber,
                    paginationParameters.PageSize)
                );
        }

        public async Task<University> GetUniversityByIdAsync(Guid id)
        {
            return await FindByCondition(university => university.Id.Equals(id))
                
                .OrderBy(x => x.Name)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UniversityExistAsync(Entities.Models.University university)
        {
            return await FindByCondition(x => x.Name == university.Name)
                .AnyAsync();
        }

        public async Task CreateUniversityAsync(Entities.Models.University university)
        {
            await CreateAsync(university);
        }

        public async Task UpdateUniversityAsync(Entities.Models.University university)
        {
            await UpdateAsync(university);
        }

        public async Task DeleteUniversityAsync(Entities.Models.University university)
        {
            await DeleteAsync(university);
        }
    }
}

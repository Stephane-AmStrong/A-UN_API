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
    public class AcademicYearRepository : RepositoryBase<AcademicYear>, IAcademicYearRepository
    {
        public AcademicYearRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<PagedList<AcademicYear>> GetAllAcademicYearsAsync(QueryStringParameters paginationParameters)
        {
            return await Task.Run(() =>
                PagedList<AcademicYear>.ToPagedList(FindAll().OrderBy(x => x.Name),
                    paginationParameters.PageNumber,
                    paginationParameters.PageSize)
                );
        }

        public async Task<AcademicYear> GetAcademicYearByIdAsync(Guid id)
        {
            return await FindByCondition(academicYear => academicYear.Id.Equals(id))
                
                .OrderBy(x => x.Name)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> AcademicYearExistAsync(Entities.Models.AcademicYear academicYear)
        {
            return await FindByCondition(x => x.Name == academicYear.Name)
                .AnyAsync();
        }

        public async Task CreateAcademicYearAsync(Entities.Models.AcademicYear academicYear)
        {
            await CreateAsync(academicYear);
        }

        public async Task UpdateAcademicYearAsync(Entities.Models.AcademicYear academicYear)
        {
            await UpdateAsync(academicYear);
        }

        public async Task DeleteAcademicYearAsync(Entities.Models.AcademicYear academicYear)
        {
            await DeleteAsync(academicYear);
        }
    }
}

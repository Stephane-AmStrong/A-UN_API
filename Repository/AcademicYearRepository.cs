using Contracts;
using Entities;
using Entities.Extensions;
using Entities.Helpers;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class AcademicYearRepository : RepositoryBase<AcademicYear>, IAcademicYearRepository
    {
        private ISortHelper<AcademicYear> _sortHelper;

        public AcademicYearRepository(
            RepositoryContext repositoryContext, 
            ISortHelper<AcademicYear> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<AcademicYear>> GetAcademicYearsAsync(AcademicYearParameters academicYearParameters)
        {
            var academicYears = Enumerable.Empty<AcademicYear>().AsQueryable();

            ApplyFilters(ref academicYears, academicYearParameters);

            PerformSearch(ref academicYears, academicYearParameters.SearchTerm);

            var sortedAcademicYears = _sortHelper.ApplySort(academicYears, academicYearParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<AcademicYear>.ToPagedList
                (
                    sortedAcademicYears,
                    academicYearParameters.PageNumber,
                    academicYearParameters.PageSize)
                );
        }

        public async Task<AcademicYear> GetOpenAcademicYearAsync()
        {
            return await FindByCondition(academicYear => academicYear.IsOpen)
                .FirstOrDefaultAsync();
        }

        public async Task<AcademicYear> GetAcademicYearByIdAsync(Guid id)
        {
            return await FindByCondition(academicYear => academicYear.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> AcademicYearExistAsync(AcademicYear academicYear)
        {
            return await FindByCondition(x => x.StartsOn == academicYear.StartsOn || x.EndsOn == academicYear.EndsOn)
                .AnyAsync();
        }

        public async Task CreateAcademicYearAsync(AcademicYear academicYear)
        {
            await CreateAsync(academicYear);
        }

        public async Task UpdateAcademicYearAsync(AcademicYear academicYear)
        {
            await UpdateAsync(academicYear);
        }

        public async Task UpdateAcademicYearAsync(IEnumerable<AcademicYear> academicYears)
        {
            await UpdateAsync(academicYears);
        }

        public async Task DeleteAcademicYearAsync(AcademicYear academicYear)
        {
            await DeleteAsync(academicYear);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<AcademicYear> academicYears, AcademicYearParameters academicYearParameters)
        {
            academicYears = FindAll();
            if (academicYearParameters.DisplaysTheOpenOneOnly)
            {
                academicYears = academicYears.Where(x => x.IsOpen);
            }

            /*
            if (academicYearParameters.MinBirthday != null)
            {
                academicYears = academicYears.Where(x => x.Birthday >= academicYearParameters.MinBirthday);
            }

            if (academicYearParameters.MaxBirthday != null)
            {
                academicYears = academicYears.Where(x => x.Birthday < academicYearParameters.MaxBirthday);
            }

            if (academicYearParameters.MinCreateAt != null)
            {
                academicYears = academicYears.Where(x => x.CreateAt >= academicYearParameters.MinCreateAt);
            }

            if (academicYearParameters.MaxCreateAt != null)
            {
                academicYears = academicYears.Where(x => x.CreateAt < academicYearParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<AcademicYear> academicYears, DateTime? searchTerm)
        {
            if (!academicYears.Any() || searchTerm == null) return;

            academicYears = academicYears.Where(x => x.StartsOn == searchTerm || x.EndsOn == searchTerm);
        }

        #endregion

    }
}

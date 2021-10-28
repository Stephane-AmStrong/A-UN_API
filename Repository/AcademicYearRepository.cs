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
        private IDataShaper<AcademicYear> _dataShaper;

        public AcademicYearRepository(
            RepositoryContext repositoryContext, 
            ISortHelper<AcademicYear> sortHelper,
            IDataShaper<AcademicYear> dataShaper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }

        public async Task<PagedList<Entity>> GetAllAcademicYearsAsync(AcademicYearParameters academicYearParameters)
        {
            var academicYears = Enumerable.Empty<AcademicYear>().AsQueryable();

            ApplyFilters(ref academicYears, academicYearParameters);

            PerformSearch(ref academicYears, academicYearParameters.SearchTerm);

            var sortedAcademicYears = _sortHelper.ApplySort(academicYears, academicYearParameters.OrderBy);
            var shapedAcademicYears = _dataShaper.ShapeData(sortedAcademicYears, academicYearParameters.Fields);

            return await Task.Run(() =>
                PagedList<Entity>.ToPagedList
                (
                    shapedAcademicYears,
                    academicYearParameters.PageNumber,
                    academicYearParameters.PageSize)
                );
        }

        public async Task<Entity> GetAcademicYearByIdAsync(Guid id, string fields)
        {
            var academicYear = FindByCondition(academicYear => academicYear.Id.Equals(id))
                .DefaultIfEmpty(new AcademicYear())
                .FirstOrDefault();

            return await Task.Run(() =>
                _dataShaper.ShapeData(academicYear, fields)
            );
        }

        public async Task<AcademicYear> GetAcademicYearByIdAsync(Guid id)
        {
            return await FindByCondition(academicYear => academicYear.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> AcademicYearExistAsync(AcademicYear academicYear)
        {
            return await FindByCondition(x => x.Name == academicYear.Name)
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

        public async Task DeleteAcademicYearAsync(AcademicYear academicYear)
        {
            await DeleteAsync(academicYear);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<AcademicYear> academicYears, AcademicYearParameters academicYearParameters)
        {
            academicYears = FindAll();
            /*
            if (!string.IsNullOrWhiteSpace(academicYearParameters.AppUserId))
            {
                academicYears = academicYears.Where(x => x.AppUserId == academicYearParameters.AppUserId);
            }

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

        private void PerformSearch(ref IQueryable<AcademicYear> academicYears, string searchTerm)
        {
            if (!academicYears.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            academicYears = academicYears.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}

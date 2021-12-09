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
    public class UniversityRepository : RepositoryBase<University>, IUniversityRepository
    {
        private ISortHelper<University> _sortHelper;
        private IDataShaper<University> _dataShaper;

        public UniversityRepository(
            RepositoryContext repositoryContext, 
            ISortHelper<University> sortHelper,
            IDataShaper<University> dataShaper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }

        public async Task<PagedList<University>> GetUniversitiesAsync(UniversityParameters universityParameters)
        {
            var universities = Enumerable.Empty<University>().AsQueryable();

            ApplyFilters(ref universities, universityParameters);

            PerformSearch(ref universities, universityParameters.SearchTerm);

            var sortedUniversities = _sortHelper.ApplySort(universities, universityParameters.OrderBy);
            //var shapedUniversities = _dataShaper.ShapeData(sortedUniversities, universityParameters.Fields);

            return await Task.Run(() =>
                PagedList<University>.ToPagedList
                (
                    sortedUniversities,
                    universityParameters.PageNumber,
                    universityParameters.PageSize)
                );
        }

        public async Task<Entity> GetUniversityByIdAsync(Guid id, string fields)
        {
            var university = FindByCondition(university => university.Id.Equals(id))
                .DefaultIfEmpty(new University())
                .FirstOrDefault();

            return await Task.Run(() =>
                _dataShaper.ShapeData(university, fields)
            );
        }

        public async Task<University> GetUniversityByIdAsync(Guid id)
        {
            return await FindByCondition(university => university.Id.Equals(id))
                .Include(x=>x.Formations).ThenInclude(x=>x.FormationLevels).ThenInclude(x=>x.Subscriptions).ThenInclude(x=>x.AppUser)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UniversityExistAsync(University university)
        {
            return await FindByCondition(x => x.Name == university.Name)
                .AnyAsync();
        }

        public async Task CreateUniversityAsync(University university)
        {
            university.CreateAt = DateTime.Now;
            await CreateAsync(university);
        }

        public async Task UpdateUniversityAsync(University university)
        {
            await UpdateAsync(university);
        }

        public async Task DeleteUniversityAsync(University university)
        {
            await DeleteAsync(university);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<University> universities, UniversityParameters universityParameters)
        {
            universities = FindAll()
                .Include(x => x.Formations).ThenInclude(x => x.FormationLevels).ThenInclude(x => x.Subscriptions).ThenInclude(x => x.AppUser);

            if (!string.IsNullOrWhiteSpace(universityParameters.ManagedByAppUserId))
            {
                universities = universities.Where(x => x.AppUserId == universityParameters.ManagedByAppUserId);
            }

            if (universityParameters.showValidatedOnesOnly)
            {
                universities = universities.Where(x => x.ValiddatedAt !=null);
            }

            /*
            if (universityParameters.MaxBirthday != null)
            {
                universities = universities.Where(x => x.Birthday < universityParameters.MaxBirthday);
            }

            if (universityParameters.MinCreateAt != null)
            {
                universities = universities.Where(x => x.CreateAt >= universityParameters.MinCreateAt);
            }

            if (universityParameters.MaxCreateAt != null)
            {
                universities = universities.Where(x => x.CreateAt < universityParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<University> universities, string searchTerm)
        {
            if (!universities.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            universities = universities.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}

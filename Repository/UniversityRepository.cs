using Contracts;
using Entities;
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

        public UniversityRepository(
            RepositoryContext repositoryContext, 
            ISortHelper<University> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<University>> GetAllUniversitiesAsync(UniversityParameters universityParameters)
        {
            var universities = Enumerable.Empty<University>().AsQueryable();

            ApplyFilters(ref universities, universityParameters);

            PerformSearch(ref universities, universityParameters.Name);

            var sortedUniversities = _sortHelper.ApplySort(universities, universityParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<University>.ToPagedList
                (
                    sortedUniversities,
                    universityParameters.PageNumber,
                    universityParameters.PageSize)
                );
        }

        private void ApplyFilters(ref IQueryable<University> universities, UniversityParameters universityParameters)
        {
            /*
                public string AppUserId { get; set; }
                public DateTime? MinBirthday { get; set; }
                public DateTime? MaxBirthday { get; set; }
                public bool ValidBirthdayRange => MaxBirthday > MinBirthday;

                public DateTime? MinCreateAt { get; set; }
                public DateTime? MaxCreateAt { get; set; }
                public bool ValidCreateAtRange => MaxCreateAt > MinCreateAt;
             */

            universities = FindAll();
            if (!string.IsNullOrWhiteSpace(universityParameters.AppUserId))
            {
                universities = universities.Where(x => x.AppUserId == universityParameters.AppUserId);
            }
            
            if (universityParameters.MinBirthday!=null)
            {
                universities = universities.Where(x => x.Birthday >= universityParameters.MinBirthday);
            }

            if (universityParameters.MaxBirthday!=null)
            {
                universities = universities.Where(x => x.Birthday < universityParameters.MaxBirthday);
            }

            if (universityParameters.MinCreateAt!=null)
            {
                universities = universities.Where(x => x.CreateAt >= universityParameters.MinCreateAt);
            }

            if (universityParameters.MaxCreateAt!=null)
            {
                universities = universities.Where(x => x.CreateAt < universityParameters.MaxCreateAt);
            }
        }
        
        private void PerformSearch(ref IQueryable<University> universities, string universityName)
        {
            if (!universities.Any() || string.IsNullOrWhiteSpace(universityName)) return;

            universities = universities.Where(x => x.Name.ToLower().Contains(universityName.Trim().ToLower()));
        }

        public async Task<University> GetUniversityByIdAsync(Guid id)
        {
            return await FindByCondition(university => university.Id.Equals(id))
                
                .OrderBy(x => x.Name)
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
    }
}

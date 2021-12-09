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
    public class PersonalFileRepository : RepositoryBase<PersonalFile>, IPersonalFileRepository
    {
        private ISortHelper<PersonalFile> _sortHelper;
        private IDataShaper<PersonalFile> _dataShaper;

        public PersonalFileRepository(
            RepositoryContext repositoryContext, 
            ISortHelper<PersonalFile> sortHelper,
            IDataShaper<PersonalFile> dataShaper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }

        public async Task<PagedList<Entity>> GetPersonalFilesAsync(PersonalFileParameters personalFileParameters)
        {
            var personalFiles = Enumerable.Empty<PersonalFile>().AsQueryable();

            ApplyFilters(ref personalFiles, personalFileParameters);

            PerformSearch(ref personalFiles, personalFileParameters.SearchTerm);

            var sortedPersonalFiles = _sortHelper.ApplySort(personalFiles, personalFileParameters.OrderBy);
            var shapedPersonalFiles = _dataShaper.ShapeData(sortedPersonalFiles, personalFileParameters.Fields);

            return await Task.Run(() =>
                PagedList<Entity>.ToPagedList
                (
                    shapedPersonalFiles,
                    personalFileParameters.PageNumber,
                    personalFileParameters.PageSize)
                );
        }

        public async Task<Entity> GetPersonalFileByIdAsync(Guid id, string fields)
        {
            var personalFile = FindByCondition(personalFile => personalFile.Id.Equals(id))
                .DefaultIfEmpty(new PersonalFile())
                .FirstOrDefault();

            return await Task.Run(() =>
                _dataShaper.ShapeData(personalFile, fields)
            );
        }

        public async Task<PersonalFile> GetPersonalFileByIdAsync(Guid id)
        {
            return await FindByCondition(personalFile => personalFile.Id.Equals(id))
                .Include(x=>x.AppUser)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> PersonalFileExistAsync(PersonalFile personalFile)
        {
            return await FindByCondition(x => x.Name == personalFile.Name && x.AppUserId == personalFile.AppUserId)
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

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<PersonalFile> personalFiles, PersonalFileParameters personalFileParameters)
        {
            personalFiles = FindAll()
                .Include(x=>x.AppUser);

            if (!string.IsNullOrWhiteSpace(personalFileParameters.OfAppUserId))
            {
                personalFiles = personalFiles.Where(x => x.AppUserId == personalFileParameters.OfAppUserId);
            }

            /*
            if (personalFileParameters.MinBirthday != null)
            {
                personalFiles = personalFiles.Where(x => x.Birthday >= personalFileParameters.MinBirthday);
            }

            if (personalFileParameters.MaxBirthday != null)
            {
                personalFiles = personalFiles.Where(x => x.Birthday < personalFileParameters.MaxBirthday);
            }

            if (personalFileParameters.MinCreateAt != null)
            {
                personalFiles = personalFiles.Where(x => x.CreateAt >= personalFileParameters.MinCreateAt);
            }

            if (personalFileParameters.MaxCreateAt != null)
            {
                personalFiles = personalFiles.Where(x => x.CreateAt < personalFileParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<PersonalFile> personalFiles, string searchTerm)
        {
            if (!personalFiles.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            personalFiles = personalFiles.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}

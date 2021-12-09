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
    public class PrerequisiteRepository : RepositoryBase<Prerequisite>, IPrerequisiteRepository
    {
        private ISortHelper<Prerequisite> _sortHelper;
        private IDataShaper<Prerequisite> _dataShaper;

        public PrerequisiteRepository(
            RepositoryContext repositoryContext, 
            ISortHelper<Prerequisite> sortHelper,
            IDataShaper<Prerequisite> dataShaper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }

        public async Task<PagedList<Entity>> GetPrerequisitesAsync(PrerequisiteParameters registrationFormLineParameters)
        {
            var registrationFormLines = Enumerable.Empty<Prerequisite>().AsQueryable();

            ApplyFilters(ref registrationFormLines, registrationFormLineParameters);

            PerformSearch(ref registrationFormLines, registrationFormLineParameters.SearchTerm);

            var sortedPrerequisites = _sortHelper.ApplySort(registrationFormLines, registrationFormLineParameters.OrderBy);
            var shapedPrerequisites = _dataShaper.ShapeData(sortedPrerequisites, registrationFormLineParameters.Fields);

            return await Task.Run(() =>
                PagedList<Entity>.ToPagedList
                (
                    shapedPrerequisites,
                    registrationFormLineParameters.PageNumber,
                    registrationFormLineParameters.PageSize)
                );
        }

        public async Task<int> GetNextNumberAsync(PrerequisiteParameters registrationFormLineParameters)
        {
            var registrationFormLinesCount = 0;

            var registrationFormLines = Enumerable.Empty<Prerequisite>().AsQueryable();
            ApplyFilters(ref registrationFormLines, registrationFormLineParameters);

            if (registrationFormLines.Any())
            {
                registrationFormLinesCount = await registrationFormLines.MaxAsync(x=>x.NumOrder);
            }

            registrationFormLinesCount++;

            return registrationFormLinesCount;
        }


        public async Task<Entity> GetPrerequisiteByIdAsync(Guid id, string fields)
        {
            var registrationFormLine = FindByCondition(registrationFormLine => registrationFormLine.Id.Equals(id))
                .Include(x => x.FormationLevel).ThenInclude(x => x.Formation).ThenInclude(x => x.University)
                .DefaultIfEmpty(new Prerequisite())
                .FirstOrDefault();

            return await Task.Run(() =>
                _dataShaper.ShapeData(registrationFormLine, fields)
            );
        }

        public async Task<Prerequisite> GetPrerequisiteByIdAsync(Guid id)
        {
            return await FindByCondition(registrationFormLine => registrationFormLine.Id.Equals(id))
                .Include(x => x.FormationLevel).ThenInclude(x => x.Formation).ThenInclude(x => x.University)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> PrerequisiteExistAsync(Prerequisite registrationFormLine)
        {
            return await FindByCondition(x => x.Name == registrationFormLine.Name &&  x.FormationLevelId == registrationFormLine.FormationLevelId)
                .AnyAsync();
        }

        public async Task CreatePrerequisiteAsync(Prerequisite registrationFormLine)
        {
            await CreateAsync(registrationFormLine);
        }

        public async Task UpdatePrerequisiteAsync(Prerequisite registrationFormLine)
        {
            await UpdateAsync(registrationFormLine);
        }

        public async Task DeletePrerequisiteAsync(Prerequisite registrationFormLine)
        {
            await DeleteAsync(registrationFormLine);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<Prerequisite> registrationFormLines, PrerequisiteParameters registrationFormLineParameters)
        {
            registrationFormLines = FindAll().Include(x=>x.FormationLevel).ThenInclude(x=>x.Formation).ThenInclude(x=>x.University);
            if (!string.IsNullOrWhiteSpace(registrationFormLineParameters.ManagedByAppUserId))
            {
                registrationFormLines = registrationFormLines.Where(x => x.FormationLevel.Formation.University.AppUserId == registrationFormLineParameters.ManagedByAppUserId);
            }

            /*
            if (registrationFormLineParameters.MinBirthday != null)
            {
                registrationFormLines = registrationFormLines.Where(x => x.Birthday >= registrationFormLineParameters.MinBirthday);
            }

            if (registrationFormLineParameters.MaxBirthday != null)
            {
                registrationFormLines = registrationFormLines.Where(x => x.Birthday < registrationFormLineParameters.MaxBirthday);
            }

            if (registrationFormLineParameters.MinCreateAt != null)
            {
                registrationFormLines = registrationFormLines.Where(x => x.CreateAt >= registrationFormLineParameters.MinCreateAt);
            }

            if (registrationFormLineParameters.MaxCreateAt != null)
            {
                registrationFormLines = registrationFormLines.Where(x => x.CreateAt < registrationFormLineParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<Prerequisite> registrationFormLines, string searchTerm)
        {
            if (!registrationFormLines.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            registrationFormLines = registrationFormLines.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}

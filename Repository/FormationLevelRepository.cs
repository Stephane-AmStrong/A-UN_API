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
    public class FormationLevelRepository : RepositoryBase<FormationLevel>, IFormationLevelRepository
    {
        private ISortHelper<FormationLevel> _sortHelper;
        private IDataShaper<FormationLevel> _dataShaper;

        public FormationLevelRepository(
            RepositoryContext repositoryContext,
            ISortHelper<FormationLevel> sortHelper,
            IDataShaper<FormationLevel> dataShaper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }

        public async Task<PagedList<Entity>> GetFormationLevelsAsync(FormationLevelParameters formationLevelParameters)
        {
            var formationLevels = Enumerable.Empty<FormationLevel>().AsQueryable();

            ApplyFilters(ref formationLevels, formationLevelParameters);

            PerformSearch(ref formationLevels, formationLevelParameters.SearchTerm);

            var sortedFormationLevels = _sortHelper.ApplySort(formationLevels, formationLevelParameters.OrderBy);
            var shapedFormationLevels = _dataShaper.ShapeData(sortedFormationLevels, formationLevelParameters.Fields);

            return await Task.Run(() =>
                PagedList<Entity>.ToPagedList
                (
                    shapedFormationLevels,
                    formationLevelParameters.PageNumber,
                    formationLevelParameters.PageSize)
                );
        }

        public async Task<Entity> GetFormationLevelByIdAsync(Guid id, string fields)
        {
            var formationLevel = FindByCondition(formationLevel => formationLevel.Id.Equals(id))
                .Include(x => x.Formation).ThenInclude(x => x.University)
                .DefaultIfEmpty(new FormationLevel())
                .FirstOrDefault();

            return await Task.Run(() =>
                _dataShaper.ShapeData(formationLevel, fields)
            );
        }

        public async Task<FormationLevel> GetFormationLevelByIdAsync(Guid id)
        {
            return await FindByCondition(formationLevel => formationLevel.Id.Equals(id))
                .Include(x => x.Prerequisites).Include(x => x.Subscriptions).ThenInclude(x => x.AppUser).Include(x => x.Formation).ThenInclude(x => x.University)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> FormationLevelExistAsync(FormationLevel formationLevel)
        {
            return await FindByCondition(x => x.FormationId == formationLevel.FormationId && x.Code == formationLevel.Code && x.Name == formationLevel.Name)
                .Include(x=>x.Prerequisites).Include(x=>x.Formation)
                .AnyAsync();
        }

        public async Task CreateFormationLevelAsync(FormationLevel formationLevel)
        {
            await CreateAsync(formationLevel);
        }

        public async Task UpdateFormationLevelAsync(FormationLevel formationLevel)
        {
            await UpdateAsync(formationLevel);
        }

        public async Task DeleteFormationLevelAsync(FormationLevel formationLevel)
        {
            await DeleteAsync(formationLevel);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<FormationLevel> formationLevels, FormationLevelParameters formationLevelParameters)
        {
            formationLevels = FindAll()
                .Include(x => x.Subscriptions).ThenInclude(x => x.AppUser).Include(x => x.Formation).ThenInclude(x => x.University);

            if (!string.IsNullOrWhiteSpace(formationLevelParameters.ManagedByAppUserId))
            {
                formationLevels = formationLevels.Where(x => x.Formation.University.AppUserId == formationLevelParameters.ManagedByAppUserId);
            }

            if (formationLevelParameters.FromUniversityId != new Guid())
            {
                formationLevels = formationLevels.Where(x => x.Formation.UniversityId == formationLevelParameters.FromUniversityId);
            }

            if (formationLevelParameters.OfFormationId != new Guid())
            {
                formationLevels = formationLevels.Where(x => x.FormationId == formationLevelParameters.OfFormationId);
            }

            /*
            if (formationLevelParameters.MinCreateAt != null)
            {
                formationLevels = formationLevels.Where(x => x.CreateAt >= formationLevelParameters.MinCreateAt);
            }

            if (formationLevelParameters.MaxCreateAt != null)
            {
                formationLevels = formationLevels.Where(x => x.CreateAt < formationLevelParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<FormationLevel> formationLevels, string searchTerm)
        {
            if (!formationLevels.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            formationLevels = formationLevels.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}

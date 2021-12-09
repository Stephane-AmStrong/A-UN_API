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
    public class FormationRepository : RepositoryBase<Formation>, IFormationRepository
    {
        private ISortHelper<Formation> _sortHelper;
        private IDataShaper<Formation> _dataShaper;

        public FormationRepository(
            RepositoryContext repositoryContext,
            ISortHelper<Formation> sortHelper,
            IDataShaper<Formation> dataShaper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }

        public async Task<PagedList<Entity>> GetFormationsAsync(FormationParameters formationParameters)
        {
            var formations = Enumerable.Empty<Formation>().AsQueryable();

            ApplyFilters(ref formations, formationParameters);

            PerformSearch(ref formations, formationParameters.SearchTerm);

            var sortedFormations = _sortHelper.ApplySort(formations, formationParameters.OrderBy);
            var shapedFormations = _dataShaper.ShapeData(sortedFormations, formationParameters.Fields);

            return await Task.Run(() =>
                PagedList<Entity>.ToPagedList
                (
                    shapedFormations,
                    formationParameters.PageNumber,
                    formationParameters.PageSize)
                );
        }

        public async Task<Entity> GetFormationByIdAsync(Guid id, string fields)
        {
            var formation = FindByCondition(formation => formation.Id.Equals(id))
                .Include(x => x.University)
                .DefaultIfEmpty(new Formation())
                .FirstOrDefault();

            return await Task.Run(() =>
                _dataShaper.ShapeData(formation, fields)
            );
        }

        public async Task<Formation> GetFormationByIdAsync(Guid id)
        {
            return await FindByCondition(formation => formation.Id.Equals(id))
                .Include(x => x.University).Include(x => x.FormationLevels).ThenInclude(x => x.Subscriptions).ThenInclude(x => x.AppUser)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> FormationExistAsync(Formation formation)
        {
            return await FindByCondition(x => x.UniversityId == formation.UniversityId &&  x.Code == formation.Code && x.Name == formation.Name)
                .AnyAsync();
        }

        public async Task CreateFormationAsync(Formation formation)
        {
            await CreateAsync(formation);
        }

        public async Task UpdateFormationAsync(Formation formation)
        {
            await UpdateAsync(formation);
        }

        public async Task DeleteFormationAsync(Formation formation)
        {
            await DeleteAsync(formation);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<Formation> formations, FormationParameters formationParameters)
        {
            formations = FindAll()
                .Include(x => x.University).Include(x => x.FormationLevels).ThenInclude(x => x.Subscriptions).ThenInclude(x => x.AppUser);

            if (!string.IsNullOrWhiteSpace(formationParameters.ManagedByAppUserId))
            {
                formations = formations.Where(x => x.University.AppUserId == formationParameters.ManagedByAppUserId);
            }

            if (formationParameters.showValidatedOnesOnly)
            {
                formations = formations.Where(x => x.University.ValiddatedAt != null);
            }

            if (formationParameters.FromUniversityId != new Guid())
            {
                formations = formations.Where(x => x.UniversityId  == formationParameters.FromUniversityId);
            }

            /*
            if (formationParameters.MinCreateAt != null)
            {
                formations = formations.Where(x => x.CreateAt >= formationParameters.MinCreateAt);
            }

            if (formationParameters.MaxCreateAt != null)
            {
                formations = formations.Where(x => x.CreateAt < formationParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<Formation> formations, string searchTerm)
        {
            if (!formations.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            formations = formations.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}

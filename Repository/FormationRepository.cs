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

        public FormationRepository(
            RepositoryContext repositoryContext,
            ISortHelper<Formation> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<Formation>> GetFormationsAsync(FormationQueryParameters formationParameters)
        {
            var formations = Enumerable.Empty<Formation>().AsQueryable();

            ApplyFilters(ref formations, formationParameters);

            PerformSearch(ref formations, formationParameters.SearchTerm);

            var sortedFormations = _sortHelper.ApplySort(formations, formationParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<Formation>.ToPagedList
                (
                    sortedFormations,
                    formationParameters.PageNumber,
                    formationParameters.PageSize)
                );
        }

        public async Task<Formation> GetFormationByIdAsync(Guid id)
        {
            return await FindByCondition(formation => formation.Id.Equals(id))
                .Include(x => x.Category).Include(x => x.University).Include(x => x.Prerequisites.OrderBy(x=>x.NumOrder)).Include(x => x.Subscriptions).ThenInclude(x => x.AppUser)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> FormationExistAsync(Formation formation)
        {
            return await FindByCondition(x => x.UniversityId == formation.UniversityId && x.Name == formation.Name)
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
        private void ApplyFilters(ref IQueryable<Formation> formations, FormationQueryParameters formationParameters)
        {
            formations = FindAll()
                .Include(x => x.Category).Include(x => x.University).Include(x => x.Prerequisites).Include(x => x.Subscriptions).ThenInclude(x => x.AppUser);

            if (!string.IsNullOrWhiteSpace(formationParameters.ManagedByAppUserId))
            {
                formations = formations.Where(x => x.University.AppUserId == formationParameters.ManagedByAppUserId);
            }

            if (formationParameters.ValidatedOnly)
            {
                formations = formations.Where(x => x.ValidatedAt != null);
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

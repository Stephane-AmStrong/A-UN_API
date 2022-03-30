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

        public async Task<PagedList<Entity>> GetPrerequisitesAsync(PrerequisiteQueryParameters rrerequisiteParameters)
        {
            var rrerequisites = Enumerable.Empty<Prerequisite>().AsQueryable();

            ApplyFilters(ref rrerequisites, rrerequisiteParameters);

            PerformSearch(ref rrerequisites, rrerequisiteParameters.SearchTerm);

            var sortedPrerequisites = _sortHelper.ApplySort(rrerequisites, rrerequisiteParameters.OrderBy);
            var shapedPrerequisites = _dataShaper.ShapeData(sortedPrerequisites, rrerequisiteParameters.Fields);

            return await Task.Run(() =>
                PagedList<Entity>.ToPagedList
                (
                    shapedPrerequisites,
                    rrerequisiteParameters.PageNumber,
                    rrerequisiteParameters.PageSize)
                );
        }

        public async Task<int> GetNextNumberAsync(PrerequisiteQueryParameters rrerequisiteParameters)
        {
            var rrerequisitesCount = 0;

            var rrerequisites = Enumerable.Empty<Prerequisite>().AsQueryable();
            ApplyFilters(ref rrerequisites, rrerequisiteParameters);

            if (rrerequisites.Any())
            {
                rrerequisitesCount = await rrerequisites.MaxAsync(x=>x.NumOrder);
            }

            rrerequisitesCount++;

            return rrerequisitesCount;
        }


        public async Task<Entity> GetPrerequisiteByIdAsync(Guid id, string fields)
        {
            var rrerequisite = FindByCondition(rrerequisite => rrerequisite.Id.Equals(id))
                .Include(x => x.Formation).ThenInclude(x => x.University)
                .DefaultIfEmpty(new Prerequisite())
                .FirstOrDefault();

            return await Task.Run(() =>
                _dataShaper.ShapeData(rrerequisite, fields)
            );
        }

        public async Task<Prerequisite> GetPrerequisiteByIdAsync(Guid id)
        {
            return await FindByCondition(rrerequisite => rrerequisite.Id.Equals(id))
                .Include(x => x.Formation).ThenInclude(x => x.University)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> PrerequisiteExistAsync(Prerequisite rrerequisite)
        {
            return await FindByCondition(x => x.Name == rrerequisite.Name &&  x.FormationId == rrerequisite.FormationId)
                .AnyAsync();
        }

        public async Task CreatePrerequisiteAsync(Prerequisite rrerequisite)
        {
            await CreateAsync(rrerequisite);
        }

        public async Task UpdatePrerequisiteAsync(Prerequisite rrerequisite)
        {
            await UpdateAsync(rrerequisite);
        }

        public async Task DeletePrerequisiteAsync(Prerequisite rrerequisite)
        {
            await DeleteAsync(rrerequisite);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<Prerequisite> rrerequisites, PrerequisiteQueryParameters rrerequisiteParameters)
        {
            rrerequisites = FindAll().Include(x=>x.Formation).ThenInclude(x=>x.University);

            if (!string.IsNullOrWhiteSpace(rrerequisiteParameters.ManagedByAppUserId))
            {
                rrerequisites = rrerequisites.Where(x => x.Formation.University.AppUserId == rrerequisiteParameters.ManagedByAppUserId);
            }

            if (rrerequisiteParameters.OfFormationId != new Guid())
            {
                rrerequisites = rrerequisites.Where(x => x.FormationId == rrerequisiteParameters.OfFormationId);
            }

            /*
            if (rrerequisiteParameters.MaxBirthday != null)
            {
                rrerequisites = rrerequisites.Where(x => x.Birthday < rrerequisiteParameters.MaxBirthday);
            }

            if (rrerequisiteParameters.MinCreateAt != null)
            {
                rrerequisites = rrerequisites.Where(x => x.CreateAt >= rrerequisiteParameters.MinCreateAt);
            }

            if (rrerequisiteParameters.MaxCreateAt != null)
            {
                rrerequisites = rrerequisites.Where(x => x.CreateAt < rrerequisiteParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<Prerequisite> rrerequisites, string searchTerm)
        {
            if (!rrerequisites.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            rrerequisites = rrerequisites.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}

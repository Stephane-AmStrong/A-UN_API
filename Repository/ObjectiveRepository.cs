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
    public class ObjectiveRepository : RepositoryBase<Objective>, IObjectiveRepository
    {
        private ISortHelper<Objective> _sortHelper;
        private IDataShaper<Objective> _dataShaper;

        public ObjectiveRepository(
            RepositoryContext repositoryContext, 
            ISortHelper<Objective> sortHelper,
            IDataShaper<Objective> dataShaper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }

        public async Task<PagedList<Entity>> GetAllObjectivesAsync(ObjectiveParameters objectiveParameters)
        {
            var objectives = Enumerable.Empty<Objective>().AsQueryable();

            ApplyFilters(ref objectives, objectiveParameters);

            PerformSearch(ref objectives, objectiveParameters.SearchTerm);

            var sortedObjectives = _sortHelper.ApplySort(objectives, objectiveParameters.OrderBy);
            var shapedObjectives = _dataShaper.ShapeData(sortedObjectives, objectiveParameters.Fields);

            return await Task.Run(() =>
                PagedList<Entity>.ToPagedList
                (
                    shapedObjectives,
                    objectiveParameters.PageNumber,
                    objectiveParameters.PageSize)
                );
        }

        public async Task<Entity> GetObjectiveByIdAsync(Guid id, string fields)
        {
            var objective = FindByCondition(objective => objective.Id.Equals(id))
                .DefaultIfEmpty(new Objective())
                .FirstOrDefault();

            return await Task.Run(() =>
                _dataShaper.ShapeData(objective, fields)
            );
        }

        public async Task<Objective> GetObjectiveByIdAsync(Guid id)
        {
            return await FindByCondition(objective => objective.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ObjectiveExistAsync(Objective objective)
        {
            return await FindByCondition(x => x.Name == objective.Name)
                .AnyAsync();
        }

        public async Task CreateObjectiveAsync(Objective objective)
        {
            await CreateAsync(objective);
        }

        public async Task UpdateObjectiveAsync(Objective objective)
        {
            await UpdateAsync(objective);
        }

        public async Task DeleteObjectiveAsync(Objective objective)
        {
            await DeleteAsync(objective);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<Objective> objectives, ObjectiveParameters objectiveParameters)
        {
            objectives = FindAll();
            /*
            if (!string.IsNullOrWhiteSpace(objectiveParameters.AppUserId))
            {
                objectives = objectives.Where(x => x.AppUserId == objectiveParameters.AppUserId);
            }

            if (objectiveParameters.MinBirthday != null)
            {
                objectives = objectives.Where(x => x.Birthday >= objectiveParameters.MinBirthday);
            }

            if (objectiveParameters.MaxBirthday != null)
            {
                objectives = objectives.Where(x => x.Birthday < objectiveParameters.MaxBirthday);
            }

            if (objectiveParameters.MinCreateAt != null)
            {
                objectives = objectives.Where(x => x.CreateAt >= objectiveParameters.MinCreateAt);
            }

            if (objectiveParameters.MaxCreateAt != null)
            {
                objectives = objectives.Where(x => x.CreateAt < objectiveParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<Objective> objectives, string searchTerm)
        {
            if (!objectives.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            objectives = objectives.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}

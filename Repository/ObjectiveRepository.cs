using Contracts;
using Entities;
using Entities.Models;
using Entities.Models.QueryParameters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ObjectiveRepository : RepositoryBase<Objective>, IObjectiveRepository
    {
        public ObjectiveRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<PagedList<Objective>> GetAllObjectivesAsync(QueryStringParameters paginationParameters)
        {
            return await Task.Run(() =>
                PagedList<Objective>.ToPagedList(FindAll().OrderBy(x => x.Name),
                    paginationParameters.PageNumber,
                    paginationParameters.PageSize)
                );
        }

        public async Task<Objective> GetObjectiveByIdAsync(Guid id)
        {
            return await FindByCondition(objective => objective.Id.Equals(id))
                
                .OrderBy(x => x.Name)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ObjectiveExistAsync(Entities.Models.Objective objective)
        {
            return await FindByCondition(x => x.Name == objective.Name)
                .AnyAsync();
        }

        public async Task CreateObjectiveAsync(Entities.Models.Objective objective)
        {
            await CreateAsync(objective);
        }

        public async Task UpdateObjectiveAsync(Entities.Models.Objective objective)
        {
            await UpdateAsync(objective);
        }

        public async Task DeleteObjectiveAsync(Entities.Models.Objective objective)
        {
            await DeleteAsync(objective);
        }
    }
}

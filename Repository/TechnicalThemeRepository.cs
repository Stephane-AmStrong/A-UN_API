using Contracts;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class TechnicalThemeRepository : RepositoryBase<TechnicalTheme>, ITechnicalThemeRepository
    {
        public TechnicalThemeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<PagedList<TechnicalTheme>> GetAllTechnicalThemesAsync(QueryStringParameters paginationParameters)
        {
            return await Task.Run(() =>
                PagedList<TechnicalTheme>.ToPagedList(FindAll().OrderBy(x => x.Name),
                    paginationParameters.PageNumber,
                    paginationParameters.PageSize)
                );
        }

        public async Task<TechnicalTheme> GetTechnicalThemeByIdAsync(Guid id)
        {
            return await FindByCondition(technicalTheme => technicalTheme.Id.Equals(id))
                
                .OrderBy(x => x.Name)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> TechnicalThemeExistAsync(Entities.Models.TechnicalTheme technicalTheme)
        {
            return await FindByCondition(x => x.Name == technicalTheme.Name)
                .AnyAsync();
        }

        public async Task CreateTechnicalThemeAsync(Entities.Models.TechnicalTheme technicalTheme)
        {
            await CreateAsync(technicalTheme);
        }

        public async Task UpdateTechnicalThemeAsync(Entities.Models.TechnicalTheme technicalTheme)
        {
            await UpdateAsync(technicalTheme);
        }

        public async Task DeleteTechnicalThemeAsync(Entities.Models.TechnicalTheme technicalTheme)
        {
            await DeleteAsync(technicalTheme);
        }
    }
}

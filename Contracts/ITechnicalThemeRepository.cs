
using Entities.Models;
using Entities.Models.QueryParameters;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ITechnicalThemeRepository
    {
        Task<PagedList<TechnicalTheme>> GetAllTechnicalThemesAsync(QueryStringParameters paginationParameters);

        Task<TechnicalTheme> GetTechnicalThemeByIdAsync(Guid id);
        Task<bool> TechnicalThemeExistAsync(TechnicalTheme technicalTheme);

        Task CreateTechnicalThemeAsync(TechnicalTheme technicalTheme);
        Task UpdateTechnicalThemeAsync(TechnicalTheme technicalTheme);
        Task DeleteTechnicalThemeAsync(TechnicalTheme technicalTheme);
    }
}

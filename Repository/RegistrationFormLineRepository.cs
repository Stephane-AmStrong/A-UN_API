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
    public class RegistrationFormLineRepository : RepositoryBase<RegistrationFormLine>, IRegistrationFormLineRepository
    {
        private ISortHelper<RegistrationFormLine> _sortHelper;
        private IDataShaper<RegistrationFormLine> _dataShaper;

        public RegistrationFormLineRepository(
            RepositoryContext repositoryContext, 
            ISortHelper<RegistrationFormLine> sortHelper,
            IDataShaper<RegistrationFormLine> dataShaper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }

        public async Task<PagedList<Entity>> GetAllRegistrationFormLinesAsync(RegistrationFormLineParameters registrationFormLineParameters)
        {
            var registrationFormLines = Enumerable.Empty<RegistrationFormLine>().AsQueryable();

            ApplyFilters(ref registrationFormLines, registrationFormLineParameters);

            PerformSearch(ref registrationFormLines, registrationFormLineParameters.SearchTerm);

            var sortedRegistrationFormLines = _sortHelper.ApplySort(registrationFormLines, registrationFormLineParameters.OrderBy);
            var shapedRegistrationFormLines = _dataShaper.ShapeData(sortedRegistrationFormLines, registrationFormLineParameters.Fields);

            return await Task.Run(() =>
                PagedList<Entity>.ToPagedList
                (
                    shapedRegistrationFormLines,
                    registrationFormLineParameters.PageNumber,
                    registrationFormLineParameters.PageSize)
                );
        }

        public async Task<Entity> GetRegistrationFormLineByIdAsync(Guid id, string fields)
        {
            var registrationFormLine = FindByCondition(registrationFormLine => registrationFormLine.Id.Equals(id))
                .DefaultIfEmpty(new RegistrationFormLine())
                .FirstOrDefault();

            return await Task.Run(() =>
                _dataShaper.ShapeData(registrationFormLine, fields)
            );
        }

        public async Task<RegistrationFormLine> GetRegistrationFormLineByIdAsync(Guid id)
        {
            return await FindByCondition(registrationFormLine => registrationFormLine.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> RegistrationFormLineExistAsync(RegistrationFormLine registrationFormLine)
        {
            return await FindByCondition(x => x.Name == registrationFormLine.Name)
                .AnyAsync();
        }

        public async Task CreateRegistrationFormLineAsync(RegistrationFormLine registrationFormLine)
        {
            await CreateAsync(registrationFormLine);
        }

        public async Task UpdateRegistrationFormLineAsync(RegistrationFormLine registrationFormLine)
        {
            await UpdateAsync(registrationFormLine);
        }

        public async Task DeleteRegistrationFormLineAsync(RegistrationFormLine registrationFormLine)
        {
            await DeleteAsync(registrationFormLine);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<RegistrationFormLine> registrationFormLines, RegistrationFormLineParameters registrationFormLineParameters)
        {
            registrationFormLines = FindAll();
            /*
            if (!string.IsNullOrWhiteSpace(registrationFormLineParameters.AppUserId))
            {
                registrationFormLines = registrationFormLines.Where(x => x.AppUserId == registrationFormLineParameters.AppUserId);
            }

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

        private void PerformSearch(ref IQueryable<RegistrationFormLine> registrationFormLines, string searchTerm)
        {
            if (!registrationFormLines.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            registrationFormLines = registrationFormLines.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}

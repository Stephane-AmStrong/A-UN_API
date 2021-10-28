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
    public class RegistrationFormRepository : RepositoryBase<RegistrationForm>, IRegistrationFormRepository
    {
        private ISortHelper<RegistrationForm> _sortHelper;
        private IDataShaper<RegistrationForm> _dataShaper;

        public RegistrationFormRepository(
            RepositoryContext repositoryContext, 
            ISortHelper<RegistrationForm> sortHelper,
            IDataShaper<RegistrationForm> dataShaper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }

        public async Task<PagedList<Entity>> GetAllRegistrationFormsAsync(RegistrationFormParameters registrationFormParameters)
        {
            var registrationForms = Enumerable.Empty<RegistrationForm>().AsQueryable();

            ApplyFilters(ref registrationForms, registrationFormParameters);

            PerformSearch(ref registrationForms, registrationFormParameters.SearchTerm);

            var sortedRegistrationForms = _sortHelper.ApplySort(registrationForms, registrationFormParameters.OrderBy);
            var shapedRegistrationForms = _dataShaper.ShapeData(sortedRegistrationForms, registrationFormParameters.Fields);

            return await Task.Run(() =>
                PagedList<Entity>.ToPagedList
                (
                    shapedRegistrationForms,
                    registrationFormParameters.PageNumber,
                    registrationFormParameters.PageSize)
                );
        }

        public async Task<Entity> GetRegistrationFormByIdAsync(Guid id, string fields)
        {
            var registrationForm = FindByCondition(registrationForm => registrationForm.Id.Equals(id))
                .DefaultIfEmpty(new RegistrationForm())
                .FirstOrDefault();

            return await Task.Run(() =>
                _dataShaper.ShapeData(registrationForm, fields)
            );
        }

        public async Task<RegistrationForm> GetRegistrationFormByIdAsync(Guid id)
        {
            return await FindByCondition(registrationForm => registrationForm.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> RegistrationFormExistAsync(RegistrationForm registrationForm)
        {
            return await FindByCondition(x => x.Name == registrationForm.Name)
                .AnyAsync();
        }

        public async Task CreateRegistrationFormAsync(RegistrationForm registrationForm)
        {
            await CreateAsync(registrationForm);
        }

        public async Task UpdateRegistrationFormAsync(RegistrationForm registrationForm)
        {
            await UpdateAsync(registrationForm);
        }

        public async Task DeleteRegistrationFormAsync(RegistrationForm registrationForm)
        {
            await DeleteAsync(registrationForm);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<RegistrationForm> registrationForms, RegistrationFormParameters registrationFormParameters)
        {
            registrationForms = FindAll();
            /*
            if (!string.IsNullOrWhiteSpace(registrationFormParameters.AppUserId))
            {
                registrationForms = registrationForms.Where(x => x.AppUserId == registrationFormParameters.AppUserId);
            }

            if (registrationFormParameters.MinBirthday != null)
            {
                registrationForms = registrationForms.Where(x => x.Birthday >= registrationFormParameters.MinBirthday);
            }

            if (registrationFormParameters.MaxBirthday != null)
            {
                registrationForms = registrationForms.Where(x => x.Birthday < registrationFormParameters.MaxBirthday);
            }

            if (registrationFormParameters.MinCreateAt != null)
            {
                registrationForms = registrationForms.Where(x => x.CreateAt >= registrationFormParameters.MinCreateAt);
            }

            if (registrationFormParameters.MaxCreateAt != null)
            {
                registrationForms = registrationForms.Where(x => x.CreateAt < registrationFormParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<RegistrationForm> registrationForms, string searchTerm)
        {
            if (!registrationForms.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            registrationForms = registrationForms.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}

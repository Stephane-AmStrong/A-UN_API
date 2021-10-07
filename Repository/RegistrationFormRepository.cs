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
    public class RegistrationFormRepository : RepositoryBase<RegistrationForm>, IRegistrationFormRepository
    {
        public RegistrationFormRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<PagedList<RegistrationForm>> GetAllRegistrationFormsAsync(QueryStringParameters paginationParameters)
        {
            return await Task.Run(() =>
                PagedList<RegistrationForm>.ToPagedList(FindAll(),
                    paginationParameters.PageNumber,
                    paginationParameters.PageSize)
                );
        }

        public async Task<RegistrationForm> GetRegistrationFormByIdAsync(Guid id)
        {
            return await FindByCondition(registrationForm => registrationForm.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> RegistrationFormExistAsync(Entities.Models.RegistrationForm registrationForm)
        {
            return await FindByCondition(x => x.Name == registrationForm.Name)
                .AnyAsync();
        }

        public async Task CreateRegistrationFormAsync(Entities.Models.RegistrationForm registrationForm)
        {
            await CreateAsync(registrationForm);
        }

        public async Task UpdateRegistrationFormAsync(Entities.Models.RegistrationForm registrationForm)
        {
            await UpdateAsync(registrationForm);
        }

        public async Task DeleteRegistrationFormAsync(Entities.Models.RegistrationForm registrationForm)
        {
            await DeleteAsync(registrationForm);
        }
    }
}

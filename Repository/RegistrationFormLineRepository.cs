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
    public class RegistrationFormLineRepository : RepositoryBase<RegistrationFormLine>, IRegistrationFormLineRepository
    {
        public RegistrationFormLineRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<PagedList<RegistrationFormLine>> GetAllRegistrationFormLinesAsync(QueryStringParameters paginationParameters)
        {
            return await Task.Run(() =>
                PagedList<RegistrationFormLine>.ToPagedList(FindAll(),
                    paginationParameters.PageNumber,
                    paginationParameters.PageSize)
                );
        }

        public async Task<RegistrationFormLine> GetRegistrationFormLineByIdAsync(Guid id)
        {
            return await FindByCondition(registrationFormLine => registrationFormLine.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> RegistrationFormLineExistAsync(Entities.Models.RegistrationFormLine registrationFormLine)
        {
            return await FindByCondition(x => x.Name == registrationFormLine.Name)
                .AnyAsync();
        }

        public async Task CreateRegistrationFormLineAsync(Entities.Models.RegistrationFormLine registrationFormLine)
        {
            await CreateAsync(registrationFormLine);
        }

        public async Task UpdateRegistrationFormLineAsync(Entities.Models.RegistrationFormLine registrationFormLine)
        {
            await UpdateAsync(registrationFormLine);
        }

        public async Task DeleteRegistrationFormLineAsync(Entities.Models.RegistrationFormLine registrationFormLine)
        {
            await DeleteAsync(registrationFormLine);
        }
    }
}

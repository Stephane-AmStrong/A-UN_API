
using Entities.Models;
using Entities.Models.QueryParameters;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRegistrationFormRepository
    {
        Task<PagedList<RegistrationForm>> GetAllRegistrationFormsAsync(QueryStringParameters paginationParameters);

        Task<RegistrationForm> GetRegistrationFormByIdAsync(Guid id);
        Task<bool> RegistrationFormExistAsync(RegistrationForm registrationForm);

        Task CreateRegistrationFormAsync(RegistrationForm registrationForm);
        Task UpdateRegistrationFormAsync(RegistrationForm registrationForm);
        Task DeleteRegistrationFormAsync(RegistrationForm registrationForm);
    }
}


using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRegistrationFormRepository
    {
        Task<PagedList<Entity>> GetAllRegistrationFormsAsync(RegistrationFormParameters registrationFormParameters);

        Task<RegistrationForm> GetRegistrationFormByIdAsync(Guid id);
        Task<bool> RegistrationFormExistAsync(RegistrationForm registrationForm);

        Task CreateRegistrationFormAsync(RegistrationForm registrationForm);
        Task UpdateRegistrationFormAsync(RegistrationForm registrationForm);
        Task DeleteRegistrationFormAsync(RegistrationForm registrationForm);
    }
}

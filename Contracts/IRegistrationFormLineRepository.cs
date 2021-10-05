
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRegistrationFormLineRepository
    {
        Task<PagedList<RegistrationFormLine>> GetAllRegistrationFormLinesAsync(QueryStringParameters paginationParameters);

        Task<RegistrationFormLine> GetRegistrationFormLineByIdAsync(Guid id);
        Task<bool> RegistrationFormLineExistAsync(RegistrationFormLine registrationFormLine);

        Task CreateRegistrationFormLineAsync(RegistrationFormLine registrationFormLine);
        Task UpdateRegistrationFormLineAsync(RegistrationFormLine registrationFormLine);
        Task DeleteRegistrationFormLineAsync(RegistrationFormLine registrationFormLine);
    }
}

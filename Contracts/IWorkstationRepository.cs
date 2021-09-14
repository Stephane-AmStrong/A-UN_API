
using Entities.Models;
using Entities.Models.QueryParameters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IWorkstationRepository
    {
        Task<PagedList<Workstation>> GetAllWorkstationsAsync(QueryStringParameters paginationParameters);

        Task<Workstation> GetWorkstationByIdAsync(Guid id);
        Task<Workstation> GetWorkstationByNameAsync(string workstationName);
        Task<bool> WorkstationExistAsync(Workstation workstation);

        Task CreateWorkstationAsync(Workstation workstation);
        Task UpdateWorkstationAsync(Workstation workstation);
        Task DeleteWorkstationAsync(Workstation workstation);
    }
}


using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IWorkstationRepository
    {
        Task<PagedList<Workstation>> GetAllWorkstationsAsync(WorkstationParameters workstationParameters);

        Task<Workstation> GetWorkstationByIdAsync(Guid id);
        Task<Workstation> GetWorkstationByNameAsync(string workstationName);
        Task<bool> WorkstationExistAsync(Workstation workstation);

        Task CreateWorkstationAsync(Workstation workstation);
        Task UpdateWorkstationAsync(Workstation workstation);
        Task DeleteWorkstationAsync(Workstation workstation);
    }
}


using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IPartnerRepository
    {
        Task<PagedList<Entity>> GetAllPartnersAsync(PartnerParameters partnerParameters);

        Task<Partner> GetPartnerByIdAsync(Guid id);
        Task<bool> PartnerExistAsync(Partner partner);

        Task CreatePartnerAsync(Partner partner);
        Task UpdatePartnerAsync(Partner partner);
        Task DeletePartnerAsync(Partner partner);
    }
}

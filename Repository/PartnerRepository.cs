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
    public class PartnerRepository : RepositoryBase<Partner>, IPartnerRepository
    {
        public PartnerRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<PagedList<Partner>> GetAllPartnersAsync(QueryStringParameters paginationParameters)
        {
            return await Task.Run(() =>
                PagedList<Partner>.ToPagedList(FindAll(),
                    paginationParameters.PageNumber,
                    paginationParameters.PageSize)
                );
        }

        public async Task<Partner> GetPartnerByIdAsync(Guid id)
        {
            return await FindByCondition(partner => partner.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> PartnerExistAsync(Entities.Models.Partner partner)
        {
            return await FindByCondition(x => x.Name == partner.Name)
                .AnyAsync();
        }

        public async Task CreatePartnerAsync(Entities.Models.Partner partner)
        {
            await CreateAsync(partner);
        }

        public async Task UpdatePartnerAsync(Entities.Models.Partner partner)
        {
            await UpdateAsync(partner);
        }

        public async Task DeletePartnerAsync(Entities.Models.Partner partner)
        {
            await DeleteAsync(partner);
        }
    }
}

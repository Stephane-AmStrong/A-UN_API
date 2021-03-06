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
    public class PartnerRepository : RepositoryBase<Partner>, IPartnerRepository
    {
        private ISortHelper<Partner> _sortHelper;

        public PartnerRepository
        (
            RepositoryContext repositoryContext, 
            ISortHelper<Partner> sortHelper
        ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<Partner>> GetPartnersAsync(PartnerQueryParameters partnerParameters)
        {
            var partners = Enumerable.Empty<Partner>().AsQueryable();

            ApplyFilters(ref partners, partnerParameters);

            PerformSearch(ref partners, partnerParameters.SearchTerm);

            var sortedPartners = _sortHelper.ApplySort(partners, partnerParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<Partner>.ToPagedList
                (
                    sortedPartners,
                    partnerParameters.PageNumber,
                    partnerParameters.PageSize)
                );
        }

        public async Task<Partner> GetPartnerByIdAsync(Guid id)
        {
            return await FindByCondition(partner => partner.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> PartnerExistAsync(Partner partner)
        {
            return await FindByCondition(x => x.Name == partner.Name)
                .AnyAsync();
        }

        public async Task CreatePartnerAsync(Partner partner)
        {
            await CreateAsync(partner);
        }

        public async Task UpdatePartnerAsync(Partner partner)
        {
            await UpdateAsync(partner);
        }

        public async Task DeletePartnerAsync(Partner partner)
        {
            await DeleteAsync(partner);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<Partner> partners, PartnerQueryParameters partnerParameters)
        {
            partners = FindAll();
            /*
            if (!string.IsNullOrWhiteSpace(partnerParameters.AppUserId))
            {
                partners = partners.Where(x => x.AppUserId == partnerParameters.AppUserId);
            }

            if (partnerParameters.MinBirthday != null)
            {
                partners = partners.Where(x => x.Birthday >= partnerParameters.MinBirthday);
            }

            if (partnerParameters.MaxBirthday != null)
            {
                partners = partners.Where(x => x.Birthday < partnerParameters.MaxBirthday);
            }

            if (partnerParameters.MinCreateAt != null)
            {
                partners = partners.Where(x => x.CreateAt >= partnerParameters.MinCreateAt);
            }

            if (partnerParameters.MaxCreateAt != null)
            {
                partners = partners.Where(x => x.CreateAt < partnerParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<Partner> partners, string searchTerm)
        {
            if (!partners.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            partners = partners.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}

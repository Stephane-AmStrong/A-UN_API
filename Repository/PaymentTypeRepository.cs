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
    public class PaymentTypeRepository : RepositoryBase<PaymentType>, IPaymentTypeRepository
    {
        public PaymentTypeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<PagedList<PaymentType>> GetAllPaymentTypesAsync(QueryStringParameters paginationParameters)
        {
            return await Task.Run(() =>
                PagedList<PaymentType>.ToPagedList(FindAll(),
                    paginationParameters.PageNumber,
                    paginationParameters.PageSize)
                );
        }

        public async Task<PaymentType> GetPaymentTypeByIdAsync(Guid id)
        {
            return await FindByCondition(paymentType => paymentType.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> PaymentTypeExistAsync(Entities.Models.PaymentType paymentType)
        {
            return await FindByCondition(x => x.Name == paymentType.Name)
                .AnyAsync();
        }

        public async Task CreatePaymentTypeAsync(Entities.Models.PaymentType paymentType)
        {
            await CreateAsync(paymentType);
        }

        public async Task UpdatePaymentTypeAsync(Entities.Models.PaymentType paymentType)
        {
            await UpdateAsync(paymentType);
        }

        public async Task DeletePaymentTypeAsync(Entities.Models.PaymentType paymentType)
        {
            await DeleteAsync(paymentType);
        }
    }
}

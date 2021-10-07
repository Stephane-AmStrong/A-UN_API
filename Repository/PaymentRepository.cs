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
    public class PaymentRepository : RepositoryBase<Payment>, IPaymentRepository
    {
        public PaymentRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<PagedList<Payment>> GetAllPaymentsAsync(QueryStringParameters paginationParameters)
        {
            return await Task.Run(() =>
                PagedList<Payment>.ToPagedList(FindAll(),
                    paginationParameters.PageNumber,
                    paginationParameters.PageSize)
                );
        }

        public async Task<Payment> GetPaymentByIdAsync(Guid id)
        {
            return await FindByCondition(payment => payment.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> PaymentExistAsync(Entities.Models.Payment payment)
        {
            return await FindByCondition(x => x.Name == payment.Name)
                .AnyAsync();
        }

        public async Task CreatePaymentAsync(Entities.Models.Payment payment)
        {
            await CreateAsync(payment);
        }

        public async Task UpdatePaymentAsync(Entities.Models.Payment payment)
        {
            await UpdateAsync(payment);
        }

        public async Task DeletePaymentAsync(Entities.Models.Payment payment)
        {
            await DeleteAsync(payment);
        }
    }
}

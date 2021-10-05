
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IPaymentRepository
    {
        Task<PagedList<Payment>> GetAllPaymentsAsync(QueryStringParameters paginationParameters);

        Task<Payment> GetPaymentByIdAsync(Guid id);
        Task<bool> PaymentExistAsync(Payment payment);

        Task CreatePaymentAsync(Payment payment);
        Task UpdatePaymentAsync(Payment payment);
        Task DeletePaymentAsync(Payment payment);
    }
}


using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IPaymentRepository
    {
        Task<PagedList<Payment>> GetPaymentsAsync(PaymentQueryParameters paymentParameters);

        Task<Payment> GetPaymentByIdAsync(Guid id);
        Task<Payment> GetLastPaymentByUserIdAsync(string id);
        Task<bool> PaymentExistAsync(Payment payment);
        Task CreatePaymentAsync(Payment payment);
        Task UpdatePaymentAsync(Payment payment);
        Task DeletePaymentAsync(Payment payment);
    }
}

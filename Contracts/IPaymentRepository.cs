
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IPaymentRepository
    {
        Task<PagedList<Entity>> GetAllPaymentsAsync(PaymentParameters paymentParameters);

        Task<Payment> GetPaymentByIdAsync(Guid id);
        Task<bool> PaymentExistAsync(Payment payment);

        Task CreatePaymentAsync(Payment payment);
        Task UpdatePaymentAsync(Payment payment);
        Task DeletePaymentAsync(Payment payment);
    }
}


using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IPaymentTypeRepository
    {
        Task<PagedList<Entity>> GetAllPaymentTypesAsync(PaymentTypeParameters paymenttypeParameters);

        Task<PaymentType> GetPaymentTypeByIdAsync(Guid id);
        Task<bool> PaymentTypeExistAsync(PaymentType paymentType);

        Task CreatePaymentTypeAsync(PaymentType paymentType);
        Task UpdatePaymentTypeAsync(PaymentType paymentType);
        Task DeletePaymentTypeAsync(PaymentType paymentType);
    }
}

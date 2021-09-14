
using Entities.Models;
using Entities.Models.QueryParameters;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IPaymentTypeRepository
    {
        Task<PagedList<PaymentType>> GetAllPaymentTypesAsync(QueryStringParameters paginationParameters);

        Task<PaymentType> GetPaymentTypeByIdAsync(Guid id);
        Task<bool> PaymentTypeExistAsync(PaymentType paymentType);

        Task CreatePaymentTypeAsync(PaymentType paymentType);
        Task UpdatePaymentTypeAsync(PaymentType paymentType);
        Task DeletePaymentTypeAsync(PaymentType paymentType);
    }
}

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
    public class PaymentRepository : RepositoryBase<Payment>, IPaymentRepository
    {
        private ISortHelper<Payment> _sortHelper;
        private IDataShaper<Payment> _dataShaper;

        public PaymentRepository(
            RepositoryContext repositoryContext, 
            ISortHelper<Payment> sortHelper,
            IDataShaper<Payment> dataShaper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }

        public async Task<PagedList<Entity>> GetPaymentsAsync(PaymentParameters paymentParameters)
        {
            var payments = Enumerable.Empty<Payment>().AsQueryable();

            ApplyFilters(ref payments, paymentParameters);

            PerformSearch(ref payments, paymentParameters.SearchTerm);

            var sortedPayments = _sortHelper.ApplySort(payments, paymentParameters.OrderBy);
            var shapedPayments = _dataShaper.ShapeData(sortedPayments, paymentParameters.Fields);

            return await Task.Run(() =>
                PagedList<Entity>.ToPagedList
                (
                    shapedPayments,
                    paymentParameters.PageNumber,
                    paymentParameters.PageSize)
                );
        }

        public async Task<Entity> GetPaymentByIdAsync(Guid id, string fields)
        {
            var payment = FindByCondition(payment => payment.Id.Equals(id))
                .DefaultIfEmpty(new Payment())
                .FirstOrDefault();

            return await Task.Run(() =>
                _dataShaper.ShapeData(payment, fields)
            );
        }

        public async Task<Payment> GetPaymentByIdAsync(Guid id)
        {
            return await FindByCondition(payment => payment.Id.Equals(id))
                .Include(x => x.AppUser).Include(x => x.AcademicYear)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> PaymentExistAsync(Payment payment)
        {
            return await FindByCondition(x => x.AcademicYearId == payment.AcademicYearId)
                .AnyAsync();
        }

        public async Task CreatePaymentAsync(Payment payment)
        {
            await CreateAsync(payment);
        }

        public async Task UpdatePaymentAsync(Payment payment)
        {
            await UpdateAsync(payment);
        }

        public async Task DeletePaymentAsync(Payment payment)
        {
            await DeleteAsync(payment);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<Payment> payments, PaymentParameters paymentParameters)
        {
            payments = FindAll()
                .Include(x=>x.AppUser).Include(x=>x.AcademicYear);
            /*
            if (!string.IsNullOrWhiteSpace(paymentParameters.AppUserId))
            {
                payments = payments.Where(x => x.AppUserId == paymentParameters.AppUserId);
            }

            if (paymentParameters.MinBirthday != null)
            {
                payments = payments.Where(x => x.Birthday >= paymentParameters.MinBirthday);
            }

            if (paymentParameters.MaxBirthday != null)
            {
                payments = payments.Where(x => x.Birthday < paymentParameters.MaxBirthday);
            }

            if (paymentParameters.MinCreateAt != null)
            {
                payments = payments.Where(x => x.CreateAt >= paymentParameters.MinCreateAt);
            }

            if (paymentParameters.MaxCreateAt != null)
            {
                payments = payments.Where(x => x.CreateAt < paymentParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<Payment> payments, string searchTerm)
        {
            if (!payments.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            payments = payments.Where(x => x.AppUser.Name.ToLower().Contains(searchTerm.Trim().ToLower()) || x.AppUser.Firstname.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}

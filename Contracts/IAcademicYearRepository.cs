
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IAcademicYearRepository
    {
        Task<PagedList<AcademicYear>> GetAllAcademicYearsAsync(QueryStringParameters paginationParameters);

        Task<AcademicYear> GetAcademicYearByIdAsync(Guid id);
        Task<bool> AcademicYearExistAsync(AcademicYear academicYear);

        Task CreateAcademicYearAsync(AcademicYear academicYear);
        Task UpdateAcademicYearAsync(AcademicYear academicYear);
        Task DeleteAcademicYearAsync(AcademicYear academicYear);
    }
}

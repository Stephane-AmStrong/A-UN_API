
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IAcademicYearRepository
    {
        Task<PagedList<AcademicYear>> GetAcademicYearsAsync(AcademicYearQueryParameters academicYearParameters);

        Task<AcademicYear> GetAcademicYearByIdAsync(Guid id);
        Task<AcademicYear> GetOpenAcademicYearAsync();
        Task<bool> AcademicYearExistAsync(AcademicYear academicYear);

        Task CreateAcademicYearAsync(AcademicYear academicYear);
        Task UpdateAcademicYearAsync(AcademicYear academicYear);
        Task UpdateAcademicYearAsync(IEnumerable<AcademicYear> academicYears);
        Task DeleteAcademicYearAsync(AcademicYear academicYear);
    }
}

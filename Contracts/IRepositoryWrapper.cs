using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryWrapper
    {
        IFileRepository File { get; }

        IAccountRepository Account { get; }
        IAuthorizationService Authorization { get; }
        IAcademicYearRepository AcademicYear { get; }
        IAppUserRepository AppUser { get; }
        IFormationRepository Formation { get; }
        IFormationLevelRepository FormationLevel { get; }
        IPersonalFileRepository PersonalFile { get; }
        IPartnerRepository Partner { get; }
        IPaymentRepository Payment { get; }
        IPrerequisiteRepository Prerequisite { get; }
        ISubscriptionRepository Subscription { get; }
        ISubscriptionLineRepository SubscriptionLine { get; }
        IUniversityRepository University { get; }
        IWorkstationRepository Workstation { get; }
        IMailRepository Mail { get; }

        string Path { set; }

        Task SaveAsync();
    }
}

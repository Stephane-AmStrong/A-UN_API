﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryWrapper
    {
        IImageRepository Image { get; }
        IAuthenticationRepository Authentication { get; }
        IAcademicYearRepository AcademicYear { get; }
        IAppUserRepository AppUser { get; }
        IBranchRepository Branch { get; }
        IBranchLevelRepository BranchLevel { get; }
        IFileRepository File { get; }
        IObjectiveRepository Objective { get; }
        IPartnerRepository Partner { get; }
        IPaymentRepository Payment { get; }
        IPaymentTypeRepository PaymentType { get; }
        IRegistrationFormRepository RegistrationForm { get; }
        IRegistrationFormLineRepository RegistrationFormLine { get; }
        ISubscriptionRepository Subscription { get; }
        ISubscriptionLineRepository SubscriptionLine { get; }
        ITechnicalThemeRepository TechnicalTheme { get; }
        IUniversityRepository University { get; }
        IWorkstationRepository Workstation { get; }
        string FolderName { set; }

        Task SaveAsync();
    }
}
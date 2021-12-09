using Contracts;
using Entities;
using Entities.Helpers;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private IFileRepository _fileRepository;
        private IAccountRepository _accountRepository;
        private IAuthorizationService _authorization;
        private IAcademicYearRepository _academicYearRepository;

        private IAppUserRepository _appUser;
        private IFormationRepository _formatione;
        private IFormationLevelRepository _formationLevel;
        private IPersonalFileRepository _personalFile;
        private IMailRepository _mail;
        private IPartnerRepository _partner;
        private IPaymentRepository _payment;
        private IPrerequisiteRepository _prerequisite;
        private ISubscriptionRepository _subscription;
        private ISubscriptionLineRepository _subscriptionLine;
        private IUniversityRepository _university;
        private IWorkstationRepository _workstation;
        private IWebHostEnvironment _webHostEnvironment;

        private readonly IConfiguration _configuration;
        private IHttpContextAccessor _httpContextAccessor;
        private IOptions<EmailSettings> _emailSettings;

        private readonly ISortHelper<AcademicYear> _academicYearSortHelper;
        private readonly ISortHelper<AppUser> _appUserSortHelper;
        private readonly ISortHelper<Formation> _formationSortHelper;
        private readonly ISortHelper<FormationLevel> _formationLevelSortHelper;
        private readonly ISortHelper<Partner> _partnerSortHelper;
        private readonly ISortHelper<Payment> _paymentSortHelper;
        private readonly ISortHelper<PersonalFile> _personalFileSortHelper;
        private readonly ISortHelper<Prerequisite> _prerequisiteSortHelper;
        private readonly ISortHelper<Subscription> _subscriptionSortHelper;
        private readonly ISortHelper<SubscriptionLine> _subscriptionLineSortHelper;
        private readonly ISortHelper<University> _universitySortHelper;
        private readonly ISortHelper<Workstation> _workstationSortHelper;

        private readonly IDataShaper<AcademicYear> _academicYearDataShaper;
        private readonly IDataShaper<Formation> _formationDataShaper;
        private readonly IDataShaper<FormationLevel> _formationLevelDataShaper;
        private readonly IDataShaper<Partner> _partnerDataShaper;
        private readonly IDataShaper<Payment> _paymentDataShaper;
        private readonly IDataShaper<PersonalFile> _personalFileDataShaper;
        private readonly IDataShaper<Prerequisite> _registrationFormDataShaper;
        private readonly IDataShaper<Prerequisite> _prerequisiteDataShaper;
        private readonly IDataShaper<Subscription> _subscriptionDataShaper;
        private readonly IDataShaper<SubscriptionLine> _subscriptionLineDataShaper;
        private readonly IDataShaper<University> _universityDataShaper;


        private RepositoryContext _repoContext;
        private UserManager<AppUser> _userManager;
        private RoleManager<Workstation> _roleManager;

        private string filePath;
        public string Path
        {
            set { filePath = value; }
        }

        public IFileRepository File
        {
            get
            {
                if (_fileRepository == null)
                {
                    _fileRepository = new FileRepository(_webHostEnvironment, filePath);
                }
                return _fileRepository;
            }
        }

        public IAccountRepository Account
        {
            get
            {
                if (_accountRepository == null)
                {
                    _accountRepository = new AccountRepository(_repoContext, _userManager, _roleManager, _configuration, _httpContextAccessor);
                }
                return _accountRepository;
            }
        }

        public IAuthorizationService Authorization
        {
            get
            {
                return _authorization;
            }
        }

        public IAcademicYearRepository AcademicYear
        {
            get
            {
                if (_academicYearRepository == null)
                {
                    _academicYearRepository = new AcademicYearRepository(_repoContext, _academicYearSortHelper);
                }
                return _academicYearRepository;
            }
        }

        public IAppUserRepository AppUser
        {
            get
            {
                if (_appUser == null)
                {
                    _appUser = new AppUserRepository(_repoContext, _appUserSortHelper, _userManager);
                }
                return _appUser;
            }
        }

        public IFormationRepository Formation
        {
            get
            {
                if (_formatione == null)
                {
                    _formatione = new FormationRepository(_repoContext, _formationSortHelper, _formationDataShaper);
                }
                return _formatione;
            }
        }

        public IFormationLevelRepository FormationLevel
        {
            get
            {
                if (_formationLevel == null)
                {
                    _formationLevel = new FormationLevelRepository(_repoContext, _formationLevelSortHelper, _formationLevelDataShaper);
                }
                return _formationLevel;
            }
        }

        public IPersonalFileRepository PersonalFile
        {
            get
            {
                if (_personalFile == null)
                {
                    _personalFile = new PersonalFileRepository(_repoContext, _personalFileSortHelper, _personalFileDataShaper);
                }
                return _personalFile;
            }
        }

        public IMailRepository Mail
        {
            get
            {
                if (_mail == null)
                {
                    _mail = new MailRepository(_emailSettings);
                }
                return _mail;
            }
        }

        public IPartnerRepository Partner
        {
            get
            {
                if (_partner == null)
                {
                    _partner = new PartnerRepository(_repoContext, _partnerSortHelper, _partnerDataShaper);
                }
                return _partner;
            }
        }

        public IPaymentRepository Payment
        {
            get
            {
                if (_payment == null)
                {
                    _payment = new PaymentRepository(_repoContext, _paymentSortHelper, _paymentDataShaper);
                }
                return _payment;
            }
        }

        public IPrerequisiteRepository Prerequisite
        {
            get
            {
                if (_prerequisite == null)
                {
                    _prerequisite = new PrerequisiteRepository(_repoContext, _prerequisiteSortHelper, _prerequisiteDataShaper);
                }
                return _prerequisite;
            }
        }

        public ISubscriptionRepository Subscription
        {
            get
            {
                if (_subscription == null)
                {
                    _subscription = new SubscriptionRepository(_repoContext, _subscriptionSortHelper, _subscriptionDataShaper);
                }
                return _subscription;
            }
        }

        public ISubscriptionLineRepository SubscriptionLine
        {
            get
            {
                if (_subscriptionLine == null)
                {
                    _subscriptionLine = new SubscriptionLineRepository(_repoContext, _subscriptionLineSortHelper, _subscriptionLineDataShaper);
                }
                return _subscriptionLine;
            }
        }

        public IUniversityRepository University
        {
            get
            {
                if (_university == null)
                {
                    _university = new UniversityRepository(_repoContext, _universitySortHelper, _universityDataShaper);
                }
                return _university;
            }
        }

        public IWorkstationRepository Workstation
        {
            get
            {
                if (_workstation == null)
                {
                    _workstation = new WorkstationRepository(_repoContext, _workstationSortHelper, _roleManager);
                }
                return _workstation;
            }
        }




        public RepositoryWrapper(
            IAuthorizationService authorization,
            UserManager<AppUser> userManager,
            RoleManager<Workstation> roleManager,
            RepositoryContext repositoryContext,
            IOptions<EmailSettings> options,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration,
            IDataShaper<AcademicYear> academicYearDataShaper,
            IDataShaper<Formation> formationDataShaper,
            IDataShaper<FormationLevel> formationLevelDataShaper,
            IDataShaper<Partner> partnerDataShaper,
            IDataShaper<Payment> paymentDataShaper,
            IDataShaper<PersonalFile> personalFileDataShaper,
            IDataShaper<Prerequisite> registrationFormDataShaper,
            IDataShaper<Prerequisite> prerequisiteDataShaper,
            IDataShaper<Subscription> subscriptionDataShaper,
            IDataShaper<SubscriptionLine> subscriptionLineDataShaper,
            IDataShaper<University> universityDataShaper,

            ISortHelper<AcademicYear> academicYearSortHelper,
            ISortHelper<AppUser> appUserSortHelper,
            ISortHelper<Formation> formationSortHelper,
            ISortHelper<FormationLevel> formationLevelSortHelper,
            ISortHelper<Partner> partnerSortHelper,
            ISortHelper<Payment> paymentSortHelper,
            ISortHelper<PersonalFile> personalFileSortHelper,
            ISortHelper<Prerequisite> prerequisiteSortHelper,
            ISortHelper<Subscription> subscriptionSortHelper,
            ISortHelper<SubscriptionLine> subscriptionLineSortHelper,
            ISortHelper<University> universitySortHelper,
            ISortHelper<Workstation> workstationSortHelper,
            IHttpContextAccessor httpContextAccessor)
        {
            _authorization = authorization;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _repoContext = repositoryContext;

            _academicYearDataShaper = academicYearDataShaper;
            _formationDataShaper = formationDataShaper;
            _formationLevelDataShaper = formationLevelDataShaper;
            _partnerDataShaper = partnerDataShaper;
            _paymentDataShaper = paymentDataShaper;
            _personalFileDataShaper = personalFileDataShaper;
            _registrationFormDataShaper = registrationFormDataShaper;
            _prerequisiteDataShaper = prerequisiteDataShaper;
            _subscriptionDataShaper = subscriptionDataShaper;
            _subscriptionLineDataShaper = subscriptionLineDataShaper;
            _universityDataShaper = universityDataShaper;
            _workstationSortHelper = workstationSortHelper;

            _academicYearSortHelper = academicYearSortHelper;
            _appUserSortHelper = appUserSortHelper;
            _formationSortHelper = formationSortHelper;
            _formationLevelSortHelper = formationLevelSortHelper;
            _partnerSortHelper = partnerSortHelper;
            _paymentSortHelper = paymentSortHelper;
            _personalFileSortHelper = personalFileSortHelper;
            _prerequisiteSortHelper = prerequisiteSortHelper;
            _subscriptionSortHelper = subscriptionSortHelper;
            _subscriptionLineSortHelper = subscriptionLineSortHelper;
            _universitySortHelper = universitySortHelper;

            _emailSettings = options;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task SaveAsync()
        {
            await _repoContext.SaveChangesAsync();
        }
    }
}

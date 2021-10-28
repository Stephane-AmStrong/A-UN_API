using Contracts;
using Entities;
using Entities.Helpers;
using Entities.Models;
using Entities.RequestFeatures;
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
        private IAuthenticationRepository _authenticationRepository;
        private IAcademicYearRepository _academicYearRepository;

        private IAppUserRepository _appUser;
        private IBranchRepository _branche;
        private IBranchLevelRepository _branchLevel;
        private IPersonalFileRepository _personalFile;
        private IObjectiveRepository _objective;
        private IMailRepository _mail;
        private IPartnerRepository _partner;
        private IPaymentRepository _payment;
        private IPaymentTypeRepository _paymentType;
        private IRegistrationFormRepository _registrationForm;
        private IRegistrationFormLineRepository _registrationFormLine;
        private ISubscriptionRepository _subscription;
        private ISubscriptionLineRepository _subscriptionLine;
        private IUniversityRepository _university;
        private IWorkstationRepository _workstation;
        private IWebHostEnvironment _webHostEnvironment;

        private readonly IConfiguration _configuration;
        private IHttpContextAccessor _httpContextAccessor;
        private IOptions<EmailSettings> _emailSettings;

        private readonly ISortHelper<AcademicYear> _academicYearSortHelper;
        private readonly ISortHelper<Branch> _branchSortHelper;
        private readonly ISortHelper<BranchLevel> _branchLevelSortHelper;
        private readonly ISortHelper<Objective> _objectiveSortHelper;
        private readonly ISortHelper<Partner> _partnerSortHelper;
        private readonly ISortHelper<Payment> _paymentSortHelper;
        private readonly ISortHelper<PaymentType> _paymentTypeSortHelper;
        private readonly ISortHelper<PersonalFile> _personalFileSortHelper;
        private readonly ISortHelper<RegistrationForm> _registrationFormSortHelper;
        private readonly ISortHelper<RegistrationFormLine> _registrationFormLineSortHelper;
        private readonly ISortHelper<Subscription> _subscriptionSortHelper;
        private readonly ISortHelper<SubscriptionLine> _subscriptionLineSortHelper;
        private readonly ISortHelper<University> _universitySortHelper;

        private readonly IDataShaper<AcademicYear> _academicYearDataShaper;
        private readonly IDataShaper<Branch> _branchDataShaper;
        private readonly IDataShaper<BranchLevel> _branchLevelDataShaper;
        private readonly IDataShaper<Objective> _objectiveDataShaper;
        private readonly IDataShaper<Partner> _partnerDataShaper;
        private readonly IDataShaper<Payment> _paymentDataShaper;
        private readonly IDataShaper<PaymentType> _paymentTypeDataShaper;
        private readonly IDataShaper<PersonalFile> _personalFileDataShaper;
        private readonly IDataShaper<RegistrationForm> _registrationFormDataShaper;
        private readonly IDataShaper<RegistrationFormLine> _registrationFormLineDataShaper;
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

        public IAuthenticationRepository Authentication
        {
            get
            {
                if (_authenticationRepository == null)
                {
                    _authenticationRepository = new AuthenticationRepository(_repoContext, _userManager, _roleManager, _configuration);
                }
                return _authenticationRepository;
            }
        }

        public IAcademicYearRepository AcademicYear
        {
            get
            {
                if (_academicYearRepository == null)
                {
                    _academicYearRepository = new AcademicYearRepository(_repoContext, _academicYearSortHelper, _academicYearDataShaper);
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
                    _appUser = new AppUserRepository(_repoContext, _userManager);
                }
                return _appUser;
            }
        }

        public IBranchRepository Branch
        {
            get
            {
                if (_branche == null)
                {
                    _branche = new BranchRepository(_repoContext, _branchSortHelper, _branchDataShaper);
                }
                return _branche;
            }
        }

        public IBranchLevelRepository BranchLevel
        {
            get
            {
                if (_branchLevel == null)
                {
                    _branchLevel = new BranchLevelRepository(_repoContext, _branchLevelSortHelper, _branchLevelDataShaper);
                }
                return _branchLevel;
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

        public IObjectiveRepository Objective
        {
            get
            {
                if (_objective == null)
                {
                    _objective = new ObjectiveRepository(_repoContext, _objectiveSortHelper, _objectiveDataShaper);
                }
                return _objective;
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

        public IPaymentTypeRepository PaymentType
        {
            get
            {
                if (_paymentType == null)
                {
                    _paymentType = new PaymentTypeRepository(_repoContext, _paymentTypeSortHelper, _paymentTypeDataShaper);
                }
                return _paymentType;
            }
        }

        public IRegistrationFormRepository RegistrationForm
        {
            get
            {
                if (_registrationForm == null)
                {
                    _registrationForm = new RegistrationFormRepository(_repoContext, _registrationFormSortHelper, _registrationFormDataShaper);
                }
                return _registrationForm;
            }
        }

        public IRegistrationFormLineRepository RegistrationFormLine
        {
            get
            {
                if (_registrationFormLine == null)
                {
                    _registrationFormLine = new RegistrationFormLineRepository(_repoContext, _registrationFormLineSortHelper, _registrationFormLineDataShaper);
                }
                return _registrationFormLine;
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
                    _workstation = new WorkstationRepository(_repoContext, _roleManager);
                }
                return _workstation;
            }
        }




        public RepositoryWrapper(
            UserManager<AppUser> userManager,
            RoleManager<Workstation> roleManager,
            RepositoryContext repositoryContext,
            IOptions<EmailSettings> options,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration,
            IDataShaper<AcademicYear> academicYearDataShaper,
            IDataShaper<Branch> branchDataShaper,
            IDataShaper<BranchLevel> branchLevelDataShaper,
            IDataShaper<Objective> objectiveDataShaper,
            IDataShaper<Partner> partnerDataShaper,
            IDataShaper<Payment> paymentDataShaper,
            IDataShaper<PaymentType> paymentTypeDataShaper,
            IDataShaper<PersonalFile> personalFileDataShaper,
            IDataShaper<RegistrationForm> registrationFormDataShaper,
            IDataShaper<RegistrationFormLine> registrationFormLineDataShaper,
            IDataShaper<Subscription> subscriptionDataShaper,
            IDataShaper<SubscriptionLine> subscriptionLineDataShaper,
            IDataShaper<University> universityDataShaper,

            ISortHelper<AcademicYear> academicYearSortHelper,
            ISortHelper<Branch> branchSortHelper,
            ISortHelper<BranchLevel> branchLevelSortHelper,
            ISortHelper<Objective> objectiveSortHelper,
            ISortHelper<Partner> partnerSortHelper,
            ISortHelper<Payment> paymentSortHelper,
            ISortHelper<PaymentType> paymentTypeSortHelper,
            ISortHelper<PersonalFile> personalFileSortHelper,
            ISortHelper<RegistrationForm> registrationFormSortHelper,
            ISortHelper<RegistrationFormLine> registrationFormLineSortHelper,
            ISortHelper<Subscription> subscriptionSortHelper,
            ISortHelper<SubscriptionLine> subscriptionLineSortHelper,
            ISortHelper<University> universitySortHelper,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _repoContext = repositoryContext;

            _academicYearDataShaper = academicYearDataShaper;
            _branchDataShaper = branchDataShaper;
            _branchLevelDataShaper = branchLevelDataShaper;
            _objectiveDataShaper = objectiveDataShaper;
            _partnerDataShaper = partnerDataShaper;
            _paymentDataShaper = paymentDataShaper;
            _paymentTypeDataShaper = paymentTypeDataShaper;
            _personalFileDataShaper = personalFileDataShaper;
            _registrationFormDataShaper = registrationFormDataShaper;
            _registrationFormLineDataShaper = registrationFormLineDataShaper;
            _subscriptionDataShaper = subscriptionDataShaper;
            _subscriptionLineDataShaper = subscriptionLineDataShaper;
            _universityDataShaper = universityDataShaper;

            _academicYearSortHelper = academicYearSortHelper;
            _branchSortHelper = branchSortHelper;
            _branchLevelSortHelper = branchLevelSortHelper;
            _objectiveSortHelper = objectiveSortHelper;
            _partnerSortHelper = partnerSortHelper;
            _paymentSortHelper = paymentSortHelper;
            _paymentTypeSortHelper = paymentTypeSortHelper;
            _personalFileSortHelper = personalFileSortHelper;
            _registrationFormSortHelper = registrationFormSortHelper;
            _registrationFormLineSortHelper = registrationFormLineSortHelper;
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

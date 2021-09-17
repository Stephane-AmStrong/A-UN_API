using Contracts;
using Entities;
using Entities.Models;
using Entities.Models.QueryParameters;
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
        private IImageRepository _imageRepository;
        private IAuthenticationRepository _authenticationRepository;
        private IAcademicYearRepository _academicYearRepository;

        private IAppUserRepository _appUser;
        private IBranchRepository _branche;
        private IBranchLevelRepository _branchLevel;
        private IFileRepository _file;
        private IObjectiveRepository _objective;
        private IMailRepository _mail;
        private IPartnerRepository _partner;
        private IPaymentRepository _payment;
        private IPaymentTypeRepository _paymentType;
        private IRegistrationFormRepository _registrationForm;
        private IRegistrationFormLineRepository _registrationFormLine;
        private ISubscriptionRepository _subscription;
        private ISubscriptionLineRepository _subscriptionLine;
        private ITechnicalThemeRepository _technicalTheme;
        private IUniversityRepository _university;
        private IWorkstationRepository _workstation;


        private IWebHostEnvironment _webHostEnvironment;


        private readonly IConfiguration _configuration;
        private IHttpContextAccessor _httpContextAccessor;

        private RepositoryContext _repoContext;
        private UserManager<AppUser> _userManager;
        private RoleManager<Workstation> _roleManager;
        private IOptions<EmailSettings> _emailSettings;

        private string folderName;
        public string FolderName
        {
            set { folderName = value; }
        }

        public IImageRepository Image
        {
            get
            {
                if (_imageRepository == null)
                {
                    _imageRepository = new ImageRepository(_webHostEnvironment, folderName);
                }
                return _imageRepository;
            }
        }

        public IAuthenticationRepository Authentication
        {
            get
            {
                if (_authenticationRepository == null)
                {
                    _authenticationRepository = new AuthenticationRepository(_repoContext, _userManager, _roleManager, _configuration, _httpContextAccessor);
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
                    _academicYearRepository = new AcademicYearRepository(_repoContext);
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
                if (_branche== null)
                {
                    _branche = new BranchRepository(_repoContext);
                }
                return _branche;
            }
        }

        public IBranchLevelRepository BranchLevel
        {
            get
            {
                if (_branchLevel== null)
                {
                    _branchLevel = new BranchLevelRepository(_repoContext);
                }
                return _branchLevel;
            }
        }

        public IFileRepository File
        {
            get
            {
                if (_file == null)
                {
                    _file = new FileRepository(_repoContext);
                }
                return _file;
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
                    _objective = new ObjectiveRepository(_repoContext);
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
                    _partner = new PartnerRepository(_repoContext);
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
                    _payment = new PaymentRepository(_repoContext);
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
                    _paymentType = new PaymentTypeRepository(_repoContext);
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
                    _registrationForm = new RegistrationFormRepository(_repoContext);
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
                    _registrationFormLine = new RegistrationFormLineRepository(_repoContext);
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
                    _subscription = new SubscriptionRepository(_repoContext);
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
                    _subscriptionLine = new SubscriptionLineRepository(_repoContext);
                }
                return _subscriptionLine;
            }
        }

        public ITechnicalThemeRepository TechnicalTheme
        {
            get
            {
                if (_technicalTheme == null)
                {
                    _technicalTheme = new TechnicalThemeRepository(_repoContext);
                }
                return _technicalTheme;
            }
        }

        public IUniversityRepository University
        {
            get
            {
                if (_university == null)
                {
                    _university = new UniversityRepository(_repoContext);
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




        public RepositoryWrapper(UserManager<AppUser> userManager, RoleManager<Workstation> roleManager, RepositoryContext repositoryContext, IOptions<EmailSettings> options, IWebHostEnvironment webHostEnvironment, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _repoContext = repositoryContext;
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

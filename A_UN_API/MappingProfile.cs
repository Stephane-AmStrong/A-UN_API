using AutoMapper;
using Entities.DataTransfertObjects;
using Entities.Models;
using System.Security.Claims;


namespace A_UN_API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Source -> Target
            CreateMap<AppUser, AppUserReadDto>()
                .ForMember(dest => dest.Email, src => src.MapFrom(src => src.UserName));

            CreateMap<AppUserReadDto, AppUser>()
                .ForMember(dest => dest.UserName, src => src.MapFrom(src => src.Email));

            CreateMap<AppUserWriteDto, AppUser>()
                .ForMember(dest => dest.UserName, src => src.MapFrom(src => src.Email))
                .ForMember(dest => dest.Name, src => src.MapFrom(src => src.Name));


            CreateMap<LoginRequestDto, LoginRequest>();
            CreateMap<AuthenticationResponse, AuthenticationResponseReadDto>();

            CreateMap<About, AboutReadDto>().ReverseMap();
            CreateMap<About, AboutWriteDto>().ReverseMap();
            CreateMap<AboutWriteDto, About>().ReverseMap();

            CreateMap<AcademicYear, AcademicYearReadDto>().ReverseMap();
            CreateMap<AcademicYearWriteDto, AcademicYear>().ReverseMap();

            CreateMap<Banner, BannerReadDto>().ReverseMap();
            CreateMap<BannerWriteDto, Banner>().ReverseMap();

            CreateMap<Category, CategoryReadDto>().ReverseMap();
            CreateMap<CategoryWriteDto, Category>().ReverseMap();

            CreateMap<Claim, ClaimReadDto>().ReverseMap();
            CreateMap<ClaimWriteDto, Claim>().ReverseMap();

            CreateMap<Formation, FormationReadDto>().ReverseMap();
            CreateMap<FormationWriteDto, Formation>().ReverseMap();

            CreateMap<Prerequisite, PrerequisiteReadDto>().ReverseMap();
            CreateMap<PrerequisiteWriteDto, Prerequisite>().ReverseMap();

            CreateMap<Partner, PartnerReadDto>().ReverseMap();
            CreateMap<PartnerWriteDto, Partner>().ReverseMap();

            CreateMap<Payment, PaymentReadDto>().ReverseMap();
            CreateMap<PaymentWriteDto, Payment>().ReverseMap();

            CreateMap<PersonalFile, PersonalFileReadDto>().ReverseMap();
            CreateMap<PersonalFileWriteDto, PersonalFile>().ReverseMap();

            CreateMap<Subscription, SubscriptionReadDto>().ReverseMap();
            CreateMap<SubscriptionWriteDto, Subscription>().ReverseMap();

            CreateMap<SubscriptionLine, SubscriptionLineReadDto>().ReverseMap();
            CreateMap<SubscriptionLineWriteDto, SubscriptionLine>().ReverseMap();

            //CreateMap<University, UniversityReadDto>()
            //    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<University, UniversityReadDto>().ReverseMap();
            CreateMap<UniversityWriteDto, University>().ReverseMap();

            CreateMap<Workstation, WorkstationReadDto>().ReverseMap();
            CreateMap<WorkstationWriteDto, Workstation>().ReverseMap();
        }
    }
}

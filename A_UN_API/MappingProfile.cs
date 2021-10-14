using AutoMapper;
using Entities.DataTransfertObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GesProdAPI
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

            CreateMap<AcademicYear, AcademicYearReadDto>();
            CreateMap<AcademicYearWriteDto, AcademicYear>();

            CreateMap<Branch, BranchReadDto>();
            CreateMap<BranchWriteDto, Branch>();

            CreateMap<BranchLevel, BranchLevelReadDto>();
            CreateMap<BranchLevelWriteDto, BranchLevel>();

            CreateMap<Objective, ObjectiveReadDto>();
            CreateMap<ObjectiveWriteDto, Objective>();

            CreateMap<Partner, PartnerReadDto>();
            CreateMap<PartnerWriteDto, Partner>();

            CreateMap<Payment, PaymentReadDto>();
            CreateMap<PaymentWriteDto, Payment>();

            CreateMap<PaymentType, PaymentTypeReadDto>();
            CreateMap<PaymentTypeWriteDto, PaymentType>();

            CreateMap<PersonalFile, PersonalFileReadDto>();
            CreateMap<PersonalFileWriteDto, PersonalFile>();

            CreateMap<RegistrationForm, RegistrationFormReadDto>();
            CreateMap<RegistrationFormWriteDto, RegistrationForm>();

            CreateMap<RegistrationFormLine, RegistrationFormLineReadDto>();
            CreateMap<RegistrationFormLineWriteDto, RegistrationFormLine>();

            CreateMap<Subscription, SubscriptionReadDto>();
            CreateMap<SubscriptionWriteDto, Subscription>();

            CreateMap<SubscriptionLine, SubscriptionLineReadDto>();
            CreateMap<SubscriptionLineWriteDto, SubscriptionLine>();

            CreateMap<TechnicalTheme, TechnicalThemeReadDto>();
            CreateMap<TechnicalThemeWriteDto, TechnicalTheme>();

            CreateMap<University, UniversityReadDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UniversityWriteDto, University>();

            CreateMap<Workstation, WorkstationReadDto>();
            CreateMap<WorkstationWriteDto, Workstation>();


            //CreateMap<University, UniversityReadDto>()
            //    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));




        }
    }
}

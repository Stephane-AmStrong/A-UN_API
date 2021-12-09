using AutoMapper;
using Entities.DataTransfertObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            CreateMap<AcademicYear, AcademicYearReadDto>();
            CreateMap<AcademicYearWriteDto, AcademicYear>();

            CreateMap<Formation, FormationReadDto>();
            CreateMap<FormationWriteDto, Formation>();

            CreateMap<FormationLevel, FormationLevelReadDto>();
            CreateMap<FormationLevelWriteDto, FormationLevel>();

            CreateMap<Partner, PartnerReadDto>();
            CreateMap<PartnerWriteDto, Partner>();

            CreateMap<Payment, PaymentReadDto>();
            CreateMap<PaymentWriteDto, Payment>();

            CreateMap<PersonalFile, PersonalFileReadDto>();
            CreateMap<PersonalFileWriteDto, PersonalFile>();

            CreateMap<Prerequisite, PrerequisiteReadDto>();
            CreateMap<PrerequisiteWriteDto, Prerequisite>();

            CreateMap<Subscription, SubscriptionReadDto>();
            CreateMap<SubscriptionWriteDto, Subscription>();

            CreateMap<SubscriptionLine, SubscriptionLineReadDto>();
            CreateMap<SubscriptionLineWriteDto, SubscriptionLine>();

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

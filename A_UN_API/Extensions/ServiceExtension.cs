using Contracts;
using Entities;
using Entities.Helpers;
using Entities.Models;
using Entities.RequestFeatures;
using LoggerServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_UN_API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => 
                builder.WithOrigins("http://localhost:5768", "http://localhost:5768")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithExposedHeaders("X-Pagination"));

            });
        }


        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {

            });
        }


        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }


        public static void ConfigureRepositoryContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config["ConnectionStrings:DbConnection"];
            IServiceCollection serviceCollections = services.AddDbContext<RepositoryContext>(options => options.UseSqlServer(connectionString));
        }


        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<ISortHelper<About>, SortHelper<About>>();
            services.AddScoped<ISortHelper<AcademicYear>, SortHelper<AcademicYear>>();
            services.AddScoped<ISortHelper<AppUser>, SortHelper<AppUser>>();
            services.AddScoped<ISortHelper<Banner>, SortHelper<Banner>>();
            services.AddScoped<ISortHelper<Category>, SortHelper<Category>>();
            services.AddScoped<ISortHelper<Formation>, SortHelper<Formation>>();
            services.AddScoped<ISortHelper<Prerequisite>, SortHelper<Prerequisite>>();
            services.AddScoped<ISortHelper<Partner>, SortHelper<Partner>>();
            services.AddScoped<ISortHelper<Payment>, SortHelper<Payment>>();
            services.AddScoped<ISortHelper<PersonalFile>, SortHelper<PersonalFile>>();
            services.AddScoped<ISortHelper<Subscription>, SortHelper<Subscription>>();
            services.AddScoped<ISortHelper<SubscriptionLine>, SortHelper<SubscriptionLine>>();
            services.AddScoped<ISortHelper<University>, SortHelper<University>>();
            services.AddScoped<ISortHelper<Workstation>, SortHelper<Workstation>>();

            services.AddScoped<IDataShaper<AcademicYear>, DataShaper<AcademicYear>>();
            services.AddScoped<IDataShaper<AppUser>, DataShaper<AppUser>>();
            services.AddScoped<IDataShaper<Formation>, DataShaper<Formation>>();
            services.AddScoped<IDataShaper<Partner>, DataShaper<Partner>>();
            services.AddScoped<IDataShaper<Payment>, DataShaper<Payment>>();
            services.AddScoped<IDataShaper<PersonalFile>, DataShaper<PersonalFile>>();
            services.AddScoped<IDataShaper<Prerequisite>, DataShaper<Prerequisite>>();
            services.AddScoped<IDataShaper<Subscription>, DataShaper<Subscription>>();
            services.AddScoped<IDataShaper<SubscriptionLine>, DataShaper<SubscriptionLine>>();
            services.AddScoped<IDataShaper<University>, DataShaper<University>>();
            services.AddScoped<IDataShaper<Workstation>, DataShaper<Workstation>>();

            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }


        public static void ConfigureJWTAuthenticationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<AppUser, Workstation>(option =>
            {
                option.Password.RequireDigit = false;
                option.Password.RequireUppercase = false;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequiredLength = 8;
                option.User.RequireUniqueEmail = true;
                option.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/ ";
            }).AddEntityFrameworkStores<RepositoryContext>()
                        .AddDefaultTokenProviders();


            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,

                    ValidAudience = configuration["AuthSettings:Audience"],
                    ValidIssuer = configuration["AuthSettings:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AuthSettings:Key"])),
                };
            });

            //services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
        }


        public static void ConfigureClaimPolicy(this IServiceCollection services)
        {
            services.AddAuthorization(option =>
            {
                option.AddPolicy("readAppUserPolicy", policy => policy.RequireClaim("readAppUser"));
                option.AddPolicy("writeAppUserPolicy", policy => policy.RequireClaim("writeAppUser"));

                option.AddPolicy("readFormationPolicy", policy => policy.RequireClaim("readFormation"));
                option.AddPolicy("writeFormationPolicy", policy => policy.RequireClaim("writeFormation"));

                option.AddPolicy("readFormationLevelPolicy", policy => policy.RequireClaim("readFormationLevel"));
                option.AddPolicy("writeFormationLevelPolicy", policy => policy.RequireClaim("writeFormationLevel"));

                option.AddPolicy("readFilePolicy", policy => policy.RequireClaim("readFile"));
                option.AddPolicy("writeFilePolicy", policy => policy.RequireClaim("writeFile"));

                option.AddPolicy("readObjectivePolicy", policy => policy.RequireClaim("readObjective"));
                option.AddPolicy("writeObjectivePolicy", policy => policy.RequireClaim("writeObjective"));

                option.AddPolicy("readPartnerPolicy", policy => policy.RequireClaim("readPartner"));
                option.AddPolicy("writePartnerPolicy", policy => policy.RequireClaim("writePartner"));

                option.AddPolicy("readPaymentPolicy", policy => policy.RequireClaim("readPayment"));
                option.AddPolicy("writePaymentPolicy", policy => policy.RequireClaim("writePayment"));

                option.AddPolicy("readPaymentTypePolicy", policy => policy.RequireClaim("readPaymentType"));
                option.AddPolicy("writePaymentTypePolicy", policy => policy.RequireClaim("writePaymentType"));

                option.AddPolicy("readPrerequisitePolicy", policy => policy.RequireClaim("readPrerequisite"));
                option.AddPolicy("writePrerequisitePolicy", policy => policy.RequireClaim("writePrerequisite"));

                option.AddPolicy("readPrerequisiteLinePolicy", policy => policy.RequireClaim("readPrerequisiteLine"));
                option.AddPolicy("writePrerequisiteLinePolicy", policy => policy.RequireClaim("writePrerequisiteLine"));

                option.AddPolicy("readSubscriptionPolicy", policy => policy.RequireClaim("readSubscription"));
                option.AddPolicy("writeSubscriptionPolicy", policy => policy.RequireClaim("writeSubscription"));

                option.AddPolicy("readSubscriptionLinePolicy", policy => policy.RequireClaim("readSubscriptionLine"));
                option.AddPolicy("writeSubscriptionLinePolicy", policy => policy.RequireClaim("writeSubscriptionLine"));

                option.AddPolicy("readTechnicalThemePolicy", policy => policy.RequireClaim("readTechnicalTheme"));
                option.AddPolicy("writeTechnicalThemePolicy", policy => policy.RequireClaim("writeTechnicalTheme"));

                option.AddPolicy("readUniversityPolicy", policy => policy.RequireClaim("readUniversity"));
                option.AddPolicy("writeUniversityPolicy", policy => policy.RequireClaim("writeUniversity"));

                option.AddPolicy("readWorkstationPolicy", policy => policy.RequireClaim("readWorkstation"));
                option.AddPolicy("writeWorkstationPolicy", policy => policy.RequireClaim("writeWorkstation"));
            });
        }


        public static void ConfigureNewtonsoftJson(this IServiceCollection services)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false).AddNewtonsoftJson(opt => {
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
        }

        public static void ConfigureMailService(this IServiceCollection services, IConfiguration config)
        {
            var emailConfig = config
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);


            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });
        }
    }
}

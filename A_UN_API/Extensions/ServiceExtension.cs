using Contracts;
using Entities;
using Entities.Models;
using Entities.Models.QueryParameters;
using LoggerServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
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
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
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
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }


        public static void ConfigureAuthenticationService(this IServiceCollection services, IConfiguration config)
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

                    ValidAudience = config["AuthSettings:Audience"],
                    ValidIssuer = config["AuthSettings:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["AuthSettings:Key"])),
                };
            });

            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
        }


        public static void ConfigureAuthorizationService(this IServiceCollection services)
        {
            services.AddAuthorization(option =>
            {
                option.AddPolicy("readAppUserPolicy", policy => policy.RequireClaim("readAppUser"));
                option.AddPolicy("writeAppUserPolicy", policy => policy.RequireClaim("writeAppUser"));

                option.AddPolicy("readBranchPolicy", policy => policy.RequireClaim("readBranch"));
                option.AddPolicy("writeBranchPolicy", policy => policy.RequireClaim("writeBranch"));

                option.AddPolicy("readBranchLevelPolicy", policy => policy.RequireClaim("readBranchLevel"));
                option.AddPolicy("writeBranchLevelPolicy", policy => policy.RequireClaim("writeBranchLevel"));

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

                option.AddPolicy("readRegistrationFormPolicy", policy => policy.RequireClaim("readRegistrationForm"));
                option.AddPolicy("writeRegistrationFormPolicy", policy => policy.RequireClaim("writeRegistrationForm"));

                option.AddPolicy("readRegistrationFormLinePolicy", policy => policy.RequireClaim("readRegistrationFormLine"));
                option.AddPolicy("writeRegistrationFormLinePolicy", policy => policy.RequireClaim("writeRegistrationFormLine"));

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
            services.Configure<EmailSettings>(config.GetSection("EmailSettings"));
        }
    }
}

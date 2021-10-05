using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Entities
{
    public class RepositoryContext : IdentityDbContext<AppUser>
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {

        }

        public DbSet<AcademicYear> AcademicYears { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<BranchLevel> BranchLevels { get; set; }
        public DbSet<PersonalFile> PersonalFiles { get; set; }
        public DbSet<Objective> Objectives { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<RegistrationForm> RegistrationForms { get; set; }
        public DbSet<RegistrationFormLine> RegistrationFormLines { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionLine> SubscriptionLines { get; set; }
        public DbSet<TechnicalTheme> TechnicalThemes { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<Workstation> Workstations { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Entity<MemberSkill>().HasKey(x => new { x.MemberId, x.SkillId, });

            //builder.Entity<Workstation>().HasData(
            //    new Workstation { Name = "SuperAdmin" },
            //    new Workstation { Name = "Administrator" },
            //    new Workstation { Name = "Students" }
            //    );

        }
    }
}

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
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Formation> Formations { get; set; }
        public DbSet<FormationLevel> FormationLevels { get; set; }
        public DbSet<PersonalFile> PersonalFiles { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Prerequisite> Prerequisites { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionLine> SubscriptionLines { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<Workstation> Workstations { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Entity<MemberSkill>().HasKey(x => new { x.MemberId, x.SkillId, });

            //builder.Entity<Workstation>().HasData(
            //    new Workstation { Name = "SuperAdmin" },
            //    new Workstation { Name = "Administrator" },
            //    new Workstation { Name = "Etudiants" }
            //    );

        }
    }
}

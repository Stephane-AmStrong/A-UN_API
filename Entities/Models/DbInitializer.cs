using Entities.RequestFeatures;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class DbInitializer
    {
        public static void Seed(RepositoryContext context)
        {
            context.Database.EnsureCreated();


            if (!context.Roles.Any(x => x.Name == "SuperAdmin"))
            {
                var superAdminRole = new Workstation
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "SuperAdmin",
                    NormalizedName = "SUPERADMIN"
                };

                context.Roles.Add(superAdminRole);

                ClaimsStore.AllClaims.ForEach(claim =>
                {
                    var claimWrapper = new ClaimWrapper();
                    claimWrapper.RoleId = superAdminRole.Id;
                    claimWrapper.InitializeFromClaim(claim);

                    context.RoleClaims.Add(claimWrapper);
                });
            }
            
            if (!context.Roles.Any(x => x.Name == "Administrator"))
            {
                var AdminRole = new Workstation
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                };

                context.Roles.Add(AdminRole);

                ClaimsStore.AllClaims.ForEach(claim =>
                {
                    var claimWrapper = new ClaimWrapper();
                    claimWrapper.RoleId = AdminRole.Id;
                    claimWrapper.InitializeFromClaim(claim);

                    context.RoleClaims.Add(claimWrapper);
                });
            }

            if (!context.Roles.Any(x => x.Name == "Etudiant"))
            {
                var EtudiantRole = new Workstation
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Etudiant",
                    NormalizedName = "ETUDIANT"
                };

                context.Roles.Add(EtudiantRole);

                ClaimsStore.EtudiantClaims.ForEach(claim =>
                {
                    var claimWrapper = new ClaimWrapper();
                    claimWrapper.RoleId = EtudiantRole.Id;
                    claimWrapper.InitializeFromClaim(claim);

                    context.RoleClaims.Add(claimWrapper);
                });
            }
            
            if (!context.Roles.Any(x => x.Name == "Administrateur d'université"))
            {
                var UniversityAdministratorRole = new Workstation
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Administrateur d'université",
                    NormalizedName = "ADMINISTRATEUR D'UNIVERSITÉ"
                };

                context.Roles.Add(UniversityAdministratorRole);

                ClaimsStore.UniversityClaims.ForEach(claim =>
                {
                    var claimWrapper = new ClaimWrapper();
                    claimWrapper.RoleId = UniversityAdministratorRole.Id;
                    claimWrapper.InitializeFromClaim(claim);

                    context.RoleClaims.Add(claimWrapper);
                });
            }
            
            if (!context.Roles.Any(x => x.Name == "Conseiller pédagogique"))
            {
                var educationalConsultantRole = new Workstation
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Conseiller pédagogique",
                    NormalizedName = "CONSEILLER PÉDAGOGIQUE"
                };

                context.Roles.Add(educationalConsultantRole);

                ClaimsStore.EducationalConsultantClaims.ForEach(claim =>
                {
                    var claimWrapper = new ClaimWrapper();
                    claimWrapper.RoleId = educationalConsultantRole.Id;
                    claimWrapper.InitializeFromClaim(claim);

                    context.RoleClaims.Add(claimWrapper);
                });
            }


            context.SaveChanges();
        }
    }
}

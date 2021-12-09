using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts
{
    public class SeedDatabaseRepository : ISeedDatabaseRepository
    {
        private readonly RepositoryContext _repoContext;

        public SeedDatabaseRepository(RepositoryContext repoContext)
        {
            _repoContext = repoContext;
        }

        public async Task<bool> seedRoles()
        {
            if (! await _repoContext.Roles.AnyAsync(x => x.Name == "SuperAdmin"))
            {
                var superAdminRole = new Workstation
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "SuperAdmin",
                    NormalizedName = "SUPERADMIN"
                };

                _repoContext.Roles.Add(superAdminRole);

                ClaimsStore.AllClaims.ForEach(claim =>
                {
                    var claimWrapper = new ClaimWrapper();
                    claimWrapper.RoleId = superAdminRole.Id;
                    claimWrapper.InitializeFromClaim(claim);

                    _repoContext.RoleClaims.Add(claimWrapper);
                });
            }

            if (!await _repoContext.Roles.AnyAsync(x => x.Name == "Administrator"))
            {
                var AdminRole = new Workstation
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                };

                _repoContext.Roles.Add(AdminRole);

                ClaimsStore.AllClaims.ForEach(claim =>
                {
                    var claimWrapper = new ClaimWrapper();
                    claimWrapper.RoleId = AdminRole.Id;
                    claimWrapper.InitializeFromClaim(claim);

                    _repoContext.RoleClaims.Add(claimWrapper);
                });
            }

            if (!await _repoContext.Roles.AnyAsync(x => x.Name == "Etudiant"))
            {
                var EtudiantRole = new Workstation
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Etudiant",
                    NormalizedName = "ETUDIANT"
                };

                _repoContext.Roles.Add(EtudiantRole);

                ClaimsStore.EtudiantClaims.ForEach(claim =>
                {
                    var claimWrapper = new ClaimWrapper();
                    claimWrapper.RoleId = EtudiantRole.Id;
                    claimWrapper.InitializeFromClaim(claim);

                    _repoContext.RoleClaims.Add(claimWrapper);
                });
            }

            if (!await _repoContext.Roles.AnyAsync(x => x.Name == "University Administrator"))
            {
                var UniversityAdministratorRole = new Workstation
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "University Administrator",
                    NormalizedName = "UNIVERSITY ADMINISTRATOR"
                };

                _repoContext.Roles.Add(UniversityAdministratorRole);

                ClaimsStore.UniversityClaims.ForEach(claim =>
                {
                    var claimWrapper = new ClaimWrapper();
                    claimWrapper.RoleId = UniversityAdministratorRole.Id;
                    claimWrapper.InitializeFromClaim(claim);

                    _repoContext.RoleClaims.Add(claimWrapper);
                });
            }

            return (await _repoContext.SaveChangesAsync() >= 0);
        }
    }
}

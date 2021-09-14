﻿using Contracts;
using Entities;
using Entities.Models;
using Entities.Models.QueryParameters;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class WorkstationRepository : RepositoryBase<Workstation>, IWorkstationRepository
    {
        private readonly RoleManager<Workstation> _roleManager;

        public WorkstationRepository(RepositoryContext repositoryContext, RoleManager<Workstation> roleManager) : base(repositoryContext)
        {
            _roleManager = roleManager;
        }

        public async Task<PagedList<Workstation>> GetAllWorkstationsAsync(QueryStringParameters paginationParameters)
        {
            return await Task.Run(() =>
                PagedList<Workstation>.ToPagedList(_roleManager.Roles.OrderBy(x => x.Name),
                    paginationParameters.PageNumber,
                    paginationParameters.PageSize)
                );
        }

        public async Task<Workstation> GetWorkstationByIdAsync(Guid id)
        {
            return await _roleManager.Roles.Where(workstation => workstation.Id.Equals(id))
                .OrderBy(x => x.Name)
                .FirstOrDefaultAsync();
        }
        

        public async Task<Workstation> GetWorkstationByNameAsync(string workstationName)
        {
            return await _roleManager.Roles.Where(workstation => workstation.Name.Equals(workstationName))
                .OrderBy(x => x.Name)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> WorkstationExistAsync(Workstation workstation)
        {
            return await _roleManager.Roles.Where(x => x.Name == workstation.Name)
                .AnyAsync();
        }

        public async Task CreateWorkstationAsync(Workstation workstation)
        {
            await _roleManager.CreateAsync(workstation);
        }

        public async Task UpdateWorkstationAsync(Workstation workstation)
        {
            await _roleManager.UpdateAsync(workstation);
        }

        public async Task DeleteWorkstationAsync(Workstation workstation)
        {
            await _roleManager.DeleteAsync(workstation);
        }
    }
}
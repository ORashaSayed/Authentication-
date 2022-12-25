using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JWT.Business.Modules.UserModule;
using JWT.Business.Modules.UserModule.Interfaces;
using JWT.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JWT.DataAccess.Repositories
{
    internal class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<RoleEntity> _roleManager;
        private readonly IMapper _mapper;
        public RoleRepository(RoleManager<RoleEntity> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public Task<List<string>> GetRolesAsync()
        {
            return _roleManager.Roles
                .Select(r => r.Name)
                .ToListAsync();
        }
        public async Task<IdentityResultWrapper> Add(string role)
        {
            var result = await _roleManager.CreateAsync(new RoleEntity { Name = role });
            return _mapper.Map<IdentityResultWrapper>(result);
        }

        public Task<bool> HasRolesAsync()
        {
            return _roleManager.Roles.AnyAsync();
        }
    }
}

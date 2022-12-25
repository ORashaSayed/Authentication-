using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using JWT.Business.Modules.UserModule;
using JWT.Business.Modules.UserModule.Interfaces;
using JWT.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JWT.DataAccess.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IMapper _mapper;

        public UserRepository(UserManager<UserEntity> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IdentityResultWrapper> AddUserAsync(User user, string password)
        {
            var entity = _mapper.Map<UserEntity>(user);
            var result = await _userManager.CreateAsync(entity, password);

            // Apply changes after saving
            if (result.Succeeded)
                _mapper.Map(entity, user);

            return _mapper.Map<IdentityResultWrapper>(result);
        }
        
        public async Task<IdentityResultWrapper> AddUserToRolesAsync(string userId, IEnumerable<string> roles)
        {
            // Load the entity from the change tracker. If not found, load it from the database.
            var entity = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.AddToRolesAsync(entity, roles);
            return _mapper.Map<IdentityResultWrapper>(result);
        }

        
        public async Task<User> GetUserByNameAsync(string username, bool includeRoles = false)
        {
            var queryable = _userManager.Users;

            if (includeRoles)
                queryable = queryable.Include(u => u.UserRoles).ThenInclude(ur => ur.Role);

            var entity = await queryable.FirstOrDefaultAsync(u => u.UserName == username);
            return _mapper.Map<User>(entity);
        }
       public async Task<bool> ValidateUserPasswordAsync(string userId, string password)
        {
            // Load the entity from the change tracker. If not found, load it from the database.
            var entity = await _userManager.FindByIdAsync(userId);
            return await _userManager.CheckPasswordAsync(entity, password);
        }
         }
}

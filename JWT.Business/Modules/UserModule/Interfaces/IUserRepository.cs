using System.Collections.Generic;
using System.Threading.Tasks;
using JWT.Business.Modules.UserModule.Requests;

namespace JWT.Business.Modules.UserModule.Interfaces
{
    public interface IUserRepository
    {
        Task<IdentityResultWrapper> AddUserAsync(User user, string password);
        Task<IdentityResultWrapper> AddUserToRolesAsync(string userId, IEnumerable<string> roles);
        Task<User> GetUserByNameAsync(string username, bool includeRoles = false);
        Task<bool> ValidateUserPasswordAsync(string userId, string password);
      
    }
}

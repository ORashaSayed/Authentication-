using System.Collections.Generic;
using System.Threading.Tasks;

namespace JWT.Business.Modules.UserModule.Interfaces
{
    public interface IRoleRepository
    {
        Task<IdentityResultWrapper> Add(string role);
        Task<List<string>> GetRolesAsync();
        Task<bool> HasRolesAsync();
    }
}

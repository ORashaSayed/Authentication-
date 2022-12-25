using System.Threading.Tasks;
using JWT.Business.Modules.UserModule.Responses;

namespace JWT.Business.Modules.UserModule.Interfaces
{
    public interface IRoleService
    {
        Task<GetRolesResponse> GetRoles();
    }
}

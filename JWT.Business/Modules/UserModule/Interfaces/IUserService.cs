using System.Threading.Tasks;
using JWT.Business.Modules.UserModule.Requests;
using JWT.Business.Modules.UserModule.Responses;

namespace JWT.Business.Modules.UserModule.Interfaces
{
    public interface IUserService
    {
        Task<UserInfoResponse> GetUserInfoAsync(GetUserInfoRequest loginUser);
    }
}

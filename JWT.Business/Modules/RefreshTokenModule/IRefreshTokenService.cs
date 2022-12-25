using System;
using System.Threading.Tasks;
using JWT.Business.Modules.RefreshTokenModule.Requests;
using JWT.Business.Modules.RefreshTokenModule.Responses;

namespace JWT.Business.Modules.RefreshTokenModule
{
    public interface IRefreshTokenService
    {
        Common.Response AddRefreshToken(AddRefreshTokenRequest refreshToken);
        Task<GetRefreshTokenResponse> GetRefreshTokenAsync(Guid jti);
        Task<GetRefreshTokenListResponse> GetRefreshTokenListAsync(string userId);
        Common.Response DeleteRefreshToken(DeleteRefreshTokenRequest storedRefreshToken);
    }
}

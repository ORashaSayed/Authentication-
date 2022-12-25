using JWT.Business.Modules.RefreshTokenModule.Models;

namespace JWT.Business.Modules.RefreshTokenModule.Responses
{
    public class GetRefreshTokenResponse : Common.Response
    {
        public RefreshToken RefreshToken { get; set; }
    }
}

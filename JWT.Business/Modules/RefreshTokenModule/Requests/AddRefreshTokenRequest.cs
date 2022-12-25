using System;

namespace JWT.Business.Modules.RefreshTokenModule.Requests
{
    public class AddRefreshTokenRequest
    {
        public string Token { set; get; }
        public DateTime Expires { get; set; }
        public string UserId { get; set; }
        public Guid Jti { get; set; }
    }
}

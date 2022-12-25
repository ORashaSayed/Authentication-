using System;

namespace JWT.Business.Modules.RefreshTokenModule.Models
{
    public class RefreshToken
    {
        public Guid Id { set; get; }
        public string Token { set; get; }
        public DateTime Expires { get; set; }
        public string UserId { get; set; }
        public Guid Jti { get; set; }
    }
}

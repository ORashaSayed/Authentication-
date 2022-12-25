using System;

namespace JWT.API.Modules.Authentication.Models
{
    public class JWTMetadata
    {
        public Guid Jti { get; set; }
        public string AuthenticationToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}
using System;
using Microsoft.IdentityModel.Tokens;

namespace JWT.API.Modules.Authentication.Models
{
    public class JwtIssuerOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public DateTime Expiration => IssuedAt.Add(ValidFor);
        public DateTime NotBefore => DateTime.UtcNow;
        public DateTime IssuedAt => DateTime.UtcNow;
        public TimeSpan ValidFor => TimeSpan.FromSeconds(ValidTime);
        public int ValidTime { set; get; }
        public SigningCredentials SigningCredentials { get; set; }
    }
}
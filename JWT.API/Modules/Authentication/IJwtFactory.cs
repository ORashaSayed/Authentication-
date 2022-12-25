using System;
using System.Collections.Generic;
using System.Security.Claims;
using JWT.API.Modules.Users.Models;

namespace JWT.API.Modules.Authentication
{
    public interface IJwtFactory
    {
        IEnumerable<Claim> GenerateClaims(UserInfo userInfo);
        string GenerateEncodedToken(IEnumerable<Claim> claims, out Guid jti);
    }
}

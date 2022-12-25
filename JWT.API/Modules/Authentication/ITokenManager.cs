using System.Collections.Generic;
using System.Security.Claims;
using JWT.API.Modules.Authentication.Models;

namespace JWT.API.Modules.Authentication
{
    public interface ITokenManager
    {
        JWTMetadata GenerateJwt(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromToken(string token, JwtIssuerOptions jwtIssuerOptions);
    }
}
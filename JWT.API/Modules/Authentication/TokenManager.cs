using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using JWT.API.Modules.Authentication.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JWT.API.Modules.Authentication
{
    internal class TokenManager : ITokenManager
    {
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;

        public TokenManager(IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions)
        {
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
        }
        public JWTMetadata GenerateJwt(IEnumerable<Claim> claims)
        {
            var authenticationToken = _jwtFactory.GenerateEncodedToken(claims, out Guid jti);
            return new JWTMetadata
            {
                Jti = jti,
                AuthenticationToken = authenticationToken,
                ExpiresIn = (int)_jwtOptions.ValidFor.TotalSeconds
            };
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        /// <summary>
        /// Decodes access token and extracts Claims Principle
        /// </summary>
        /// <exception cref="SecurityTokenException">Invalid token</exception>
        public ClaimsPrincipal GetPrincipalFromToken(string token, JwtIssuerOptions jwtIssuerOptions)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = jwtIssuerOptions.Audience,
                ValidateIssuer = true,
                ValidIssuer = jwtIssuerOptions.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = jwtIssuerOptions.SigningCredentials.Key,
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            // Checking here for security algorithm to prevent exchanging of fake token (unsigned token) with real one
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}

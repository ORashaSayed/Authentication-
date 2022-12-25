using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using JWT.API.Modules.Authentication.Models;
using JWT.API.Modules.Users.Models;
using Microsoft.Extensions.Options;

namespace JWT.API.Modules.Authentication
{
    public class JwtFactory : IJwtFactory
    {
        private readonly JwtIssuerOptions _jwtOptions;

        public JwtFactory(IOptions<JwtIssuerOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
        }
        public string GenerateEncodedToken(IEnumerable<Claim> claims, out Guid jti)
        {
            jti = Guid.NewGuid();

            var tokenClaims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Jti, jti.ToString())
            };

            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: FilterClaims(claims).Concat(tokenClaims),
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public IEnumerable<Claim> GenerateClaims(UserInfo userInfo)
        {
            var claims = new List<Claim>
            {
                 new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                 new Claim(AuthenticationConstants.JwtClaimIdentifiers.Id, userInfo.Id),
                 new Claim(AuthenticationConstants.JwtClaimIdentifiers.Email, userInfo.Email),
                 new Claim(AuthenticationConstants.JwtClaimIdentifiers.Email, userInfo.Region)
            };

            foreach (string role in userInfo.Roles)
            {
                claims.Add(new Claim(AuthenticationConstants.JwtClaimIdentifiers.Roles, role));
            }

            return claims;
        }

        private IEnumerable<Claim> FilterClaims(IEnumerable<Claim> claims)
        {
            var excludedTypes = new[] { JwtRegisteredClaimNames.Iat, JwtRegisteredClaimNames.Jti, JwtRegisteredClaimNames.Aud };
            return claims.Where(c => excludedTypes.All(x => x != c.Type));
        }

        private void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }
        }

        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);
    }
}

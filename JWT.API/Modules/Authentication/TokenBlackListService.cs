using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using JWT.API.Modules.Authentication.Models;
using JWT.Caching;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace JWT.API.Modules.Authentication
{
    internal class TokenBlacklistService : ITokenBlacklistService
    {
        private readonly IMemoryCaching _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenManager _tokenManager;
        private readonly JwtIssuerOptions _jwtOptions;

        public TokenBlacklistService(IMemoryCaching cache,
            IHttpContextAccessor httpContextAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            ITokenManager tokenManager)
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
            _tokenManager = tokenManager;
            _jwtOptions = jwtOptions.Value;
        }

        public void Add(string jti)
        {
            _cache.Set(jti, string.Empty, new MemoryCachingOptions
            {
                AbsoluteExpirationRelativeToNow = _jwtOptions.ValidFor
            });
        }

        public void AddCurrentJti()
        {
            var jti = GetCurrentAccessTokenJti();
            if (!string.IsNullOrWhiteSpace(jti))
                Add(jti);
        }

        public bool IsBlackListed(string jti)
        {
            if (string.IsNullOrWhiteSpace(jti))
                return false;

            return _cache.TryGetValue(jti, out string _);
        }

        public bool IsCurrentJtiBlackListed()
        {
            return IsBlackListed(GetCurrentAccessTokenJti());
        }

        private string GetCurrentAccessTokenJti()
        {
            var headers = _httpContextAccessor.HttpContext.Request.Headers;
            if (headers.TryGetValue("authorization", out StringValues authorization) &&
                !StringValues.IsNullOrEmpty(authorization))
            {
                return GetAccessTokenJti(authorization.First());
            }
            return string.Empty;
        }

        private string GetAccessTokenJti(string accessToken)
        {
            if (!string.IsNullOrEmpty(accessToken))
            {
                accessToken = accessToken.Split(" ").Last();
                var jti = _tokenManager
                    .GetPrincipalFromToken(accessToken, _jwtOptions)
                    .FindFirstValue(JwtRegisteredClaimNames.Jti);

                return jti;
            }
            return string.Empty;
        }
    }
}

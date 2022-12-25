using JWT.API.Modules.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Net;
using System.Threading.Tasks;
namespace JWT.API.Middlewares
{
    public class TokenBlacklistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITokenBlacklistService _tokenBlacklistService;
        private readonly ILogger _logger;

        public TokenBlacklistMiddleware(RequestDelegate next,
            ITokenBlacklistService tokenBlacklistService,
            ILogger logger)
        {
            _next = next;
            _tokenBlacklistService = tokenBlacklistService;
            _logger = logger.ForContext<TokenBlacklistMiddleware>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if (!_tokenBlacklistService.IsCurrentJtiBlackListed())
                {
                    await _next(context);
                    return;
                }
            }
            catch (SecurityTokenException securityException)
            {
                _logger.Fatal(securityException, "An error has occurred while extracting token information.");
            }

            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}

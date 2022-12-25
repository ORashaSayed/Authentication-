using JWT.API.Modules.Authentication;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JWT.API.Helpers
{
    // Used to change role authorization from the default to our roles key
    // Because exchange token replaces token keys with its default keys which doesn't contain our roles key. 
    // so, we prevented default keys replacement and used our roles key for authorization.
    public class CustomClaimsPrincipal : ClaimsPrincipal
    {
        public CustomClaimsPrincipal(ClaimsPrincipal principal) : base(principal) { }
        public override bool IsInRole(string role)
        {
            return Claims.Any(x => x.Type == AuthenticationConstants.JwtClaimIdentifiers.Roles && x.Value == role);
        }

        public string GetUserId()
        {
            return Claims.First(x => x.Type == AuthenticationConstants.JwtClaimIdentifiers.Id).Value;
        }
    }

    public class ClaimsTransformer : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var customPrincipal = new CustomClaimsPrincipal(principal) as ClaimsPrincipal;
            return Task.FromResult(customPrincipal);
        }
    }
}

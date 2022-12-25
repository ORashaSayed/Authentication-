using System.Collections.Generic;
using JWT.Business.Modules.RefreshTokenModule.Models;

namespace JWT.Business.Modules.RefreshTokenModule.Responses
{
    public class GetRefreshTokenListResponse : Common.Response
    {
        public IEnumerable<RefreshToken> RefreshTokens { set; get; }
    }
}

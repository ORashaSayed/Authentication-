using System;

namespace JWT.Business.Modules.RefreshTokenModule.Requests
{
    public class DeleteRefreshTokenRequest
    {
        public Guid RefreshTokenId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JWT.Business.Modules.RefreshTokenModule.Models;

namespace JWT.Business.Modules.RefreshTokenModule
{
    public interface IRefreshTokenRepository
    {
        void AddRefreshToken(RefreshToken refreshToken);
        Task<RefreshToken> GetRefreshTokenAsync(Guid jti);
        void DeleteRefreshToken(RefreshToken refreshToken);
        Task<List<RefreshToken>> GetUserRefreshTokensAsync(string userId);
        void DeleteRefreshTokenList(IEnumerable<RefreshToken> refreshTokens);
    }
}

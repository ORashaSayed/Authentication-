using AutoMapper;
using JWT.Business.Modules.RefreshTokenModule;
using JWT.Business.Modules.RefreshTokenModule.Models;
using JWT.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT.DataAccess.Repositories
{
    internal class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public RefreshTokenRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<RefreshToken> GetRefreshTokenAsync(Guid jti)
        {
            var refreshTokenEntity = await _dbContext.RefreshTokens
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Jti == jti);
            return _mapper.Map<RefreshToken>(refreshTokenEntity);
        }

        public void AddRefreshToken(RefreshToken refreshToken)
        {
            var refreshTokenEntity = _mapper.Map<RefreshTokenEntity>(refreshToken);
            _dbContext.RefreshTokens.Add(refreshTokenEntity);
        }

        public void DeleteRefreshToken(RefreshToken refreshToken)
        {
            var refreshTokenEntity = _mapper.Map<RefreshTokenEntity>(refreshToken);
            _dbContext.RefreshTokens.Remove(refreshTokenEntity);
        }

        public async Task<List<RefreshToken>> GetUserRefreshTokensAsync(string userId)
        {
            var refreshTokenEntities = await _dbContext.RefreshTokens
                .Where(r => r.UserId == userId)
                .ToListAsync();
            return _mapper.Map<List<RefreshToken>>(refreshTokenEntities);
        }

        public void DeleteRefreshTokenList(IEnumerable<RefreshToken> refreshTokens)
        {
            var refreshTokenEntities = _mapper.Map<IEnumerable<RefreshTokenEntity>>(refreshTokens);
            _dbContext.RefreshTokens.RemoveRange(refreshTokenEntities);
        }
    }
}

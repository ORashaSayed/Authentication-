using System;
using System.Threading.Tasks;
using AutoMapper;
using JWT.Business.Modules.RefreshTokenModule.Models;
using JWT.Business.Modules.RefreshTokenModule.Requests;
using JWT.Business.Modules.RefreshTokenModule.Responses;
using Serilog;

namespace JWT.Business.Modules.RefreshTokenModule
{
    internal class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository,
            IMapper mapper,
            ILogger logger)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _mapper = mapper;
            _logger = logger.ForContext<RefreshTokenService>();
        }
        public Common.Response AddRefreshToken(AddRefreshTokenRequest request)
        {
            try
            {
                var refreshToken = _mapper.Map<RefreshToken>(request);
                _refreshTokenRepository.AddRefreshToken(refreshToken);
                return new Common.Response { Succeeded = true };
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "An error occurred while adding refresh token: {token} with jti: {jti} for user: {userId}.",
                    request.Token, request.Jti, request.UserId);
                return new Common.Response { Succeeded = false, Reason = "An error occurred while adding refresh token." };
            }
        }

        public async Task<GetRefreshTokenResponse> GetRefreshTokenAsync(Guid jti)
        {
            try
            {
                var refreshToken = await _refreshTokenRepository.GetRefreshTokenAsync(jti);
                GetRefreshTokenResponse response;
                if (refreshToken == null)
                {
                    _logger.Error("Refresh token with jti:{jti} was not found.", jti);

                    response = new GetRefreshTokenResponse
                    {
                        Succeeded = false,
                        Reason = "Refresh token not found."
                    };
                }
                else
                {
                    response = new GetRefreshTokenResponse
                    {
                        Succeeded = true,
                        RefreshToken = refreshToken
                    };
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "An error occurred while retrieving refresh token with jti: {jti}.", jti);
                return new GetRefreshTokenResponse
                {
                    Succeeded = false,
                    Reason = "An error occurred while retrieving refresh token."
                };
            }
        }

        public Common.Response DeleteRefreshToken(DeleteRefreshTokenRequest request)
        {
            try
            {
                var refreshToken = _mapper.Map<RefreshToken>(request);
                _refreshTokenRepository.DeleteRefreshToken(refreshToken);
                return new Common.Response { Succeeded = true };
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "An error occurred while deleting refresh token with id: {id}.", request.RefreshTokenId);
                return new Common.Response { Succeeded = false, Reason = "An error occurred while deleting refresh token." };
            }
        }

        public async Task<GetRefreshTokenListResponse> GetRefreshTokenListAsync(string userId)
        {
            try
            {
                var tokens = await _refreshTokenRepository.GetUserRefreshTokensAsync(userId);
                return new GetRefreshTokenListResponse
                {
                    RefreshTokens = tokens,
                    Succeeded = true
                };
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "An error occurred while retrieving refresh tokens for user: {id}.", userId);
                return new GetRefreshTokenListResponse
                {
                    Succeeded = false,
                    Reason = "An error occurred while retrieving refresh tokens."
                };
            }
        }
    }
}

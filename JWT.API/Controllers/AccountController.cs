using AutoMapper;
using JWT.API.Modules.Authentication;
using JWT.API.Modules.Authentication.Models;
using JWT.API.Modules.Authentication.ViewModels;
using JWT.API.Modules.Users.Models;
using JWT.API.Modules.Users.ViewModels;
using JWT.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using JWT.Business.Modules.RefreshTokenModule;
using JWT.Business.Modules.RefreshTokenModule.Requests;
using JWT.Business.Modules.UserModule.Interfaces;
using JWT.Business.Modules.UserModule.Requests;

namespace JWT.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/Account")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly ITokenBlacklistService _tokenBlacklistService;
        private readonly IMapper _mapper;
        private readonly IJwtFactory _jwtFactory;
        private readonly IUnitOfWork _uow;
        private readonly ITokenManager _tokenManager;
        private readonly ILogger _logger;
        private readonly RefreshTokenOptions _refreshTokenOptions;
        private readonly JwtIssuerOptions _jwtOptions;

        public AccountController(
             IUserService userService,
             IRefreshTokenService refreshTokenService,
             ITokenBlacklistService tokenBlacklistService,
             IJwtFactory jwtFactory,
             ITokenManager tokenManager,
             IUnitOfWork uow,
             IOptions<RefreshTokenOptions> refreshTokenOptions,
             IOptions<JwtIssuerOptions> jwtOptions,
             ILogger logger,
             IMapper mapper
            )
        {
            _userService = userService;
            _refreshTokenService = refreshTokenService;
            _tokenBlacklistService = tokenBlacklistService;
            _mapper = mapper;
            _jwtFactory = jwtFactory;
            _uow = uow;
            _tokenManager = tokenManager;
            _refreshTokenOptions = refreshTokenOptions.Value;
            _jwtOptions = jwtOptions.Value;
            _logger = logger;

        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userInfoResponse = await _userService.GetUserInfoAsync(_mapper.Map<GetUserInfoRequest>(loginViewModel));
                if (!userInfoResponse.Succeeded)
                {
                    return BadRequest(userInfoResponse);
                }

                var userInfo = _mapper.Map<UserInfo>(userInfoResponse);
                var claims = _jwtFactory.GenerateClaims(userInfo);
                var (jwt, refreshToken) = CreateTokens(userInfo.Id, claims);

                await _uow.SaveChangesAsync();

                return Ok(new TokenViewModel
                {
                    AccessToken = jwt.AuthenticationToken,
                    ExpiresIn = jwt.ExpiresIn,
                    RefreshToken = refreshToken
                });
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "An error has been occurred while authenticating user: {username}", loginViewModel.Username);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error has been occurred while authentication.");
            }
        }

      
        [HttpPost("Signout")]
        public async Task<IActionResult> Signout()
        {
            try
            {
                // Add current access token to black list
                _tokenBlacklistService.AddCurrentJti();

                var jti = Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Jti));
                var response = await _refreshTokenService.GetRefreshTokenAsync(jti);
                if (!response.Succeeded)
                {
                    _logger.Error("No refresh token found while signing out user: {userId}");
                }
                else
                {
                    _refreshTokenService.DeleteRefreshToken(new DeleteRefreshTokenRequest { RefreshTokenId = response.RefreshToken.Id });
                    await _uow.SaveChangesAsync();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "An error occurred while signing out user: {userId}",
                    User.FindFirstValue(AuthenticationConstants.JwtClaimIdentifiers.Id));
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while signing out.");
            }
        }

        [AllowAnonymous]
        [HttpPost("ExchangeToken")]
        public async Task<IActionResult> ExchangeRefreshToken(ExchangeTokenViewModel exchangeTokenViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var principal = _tokenManager.GetPrincipalFromToken(exchangeTokenViewModel.AccessToken, _jwtOptions);
                var userId = principal.FindFirstValue(AuthenticationConstants.JwtClaimIdentifiers.Id);
                var jti = Guid.Parse(principal.FindFirstValue(JwtRegisteredClaimNames.Jti));
                var refreshTokenResponse = await _refreshTokenService.GetRefreshTokenAsync(jti);

                if (!refreshTokenResponse.Succeeded ||
                    refreshTokenResponse.RefreshToken.Token != exchangeTokenViewModel.RefreshToken)
                {
                    _logger.Error("Refresh Token does not match for user: {userId}.", userId);
                    return BadRequest(refreshTokenResponse.Reason);
                }

                var deleteRefreshTokenRequest = new DeleteRefreshTokenRequest { RefreshTokenId = refreshTokenResponse.RefreshToken.Id };
                _refreshTokenService.DeleteRefreshToken(deleteRefreshTokenRequest);

                if (refreshTokenResponse.RefreshToken.Expires < DateTime.UtcNow)
                {
                    await _uow.SaveChangesAsync();
                    _logger.Error("Cannot exchange expired refresh token for user: {userId}.", userId);
                    return BadRequest("Expired Refresh Token.");
                }

                var (jwt, refreshToken) = CreateTokens(userId, principal.Claims);

                await _uow.SaveChangesAsync();

                return Ok(new TokenViewModel
                {
                    AccessToken = jwt.AuthenticationToken,
                    ExpiresIn = jwt.ExpiresIn,
                    RefreshToken = refreshToken
                });
            }
            catch (SecurityTokenException securityTokenException)
            {
                _logger.Fatal(securityTokenException, "Invalid token.");
                return BadRequest("Invalid token.");
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "An error occurred while exchanging refresh token.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while exchanging refresh token.");
            }
        }

    
        #region NonActions


        [NonAction]
        private (JWTMetadata Jwt, string RefreshToken) CreateTokens(string userId, IEnumerable<Claim> claims)
        {
            var jwt = _tokenManager.GenerateJwt(claims);
            var refreshToken = _tokenManager.GenerateRefreshToken();

            var refreshTokenModel = new AddRefreshTokenRequest
            {
                Jti = jwt.Jti,
                Expires = _refreshTokenOptions.ExpirationTime,
                Token = refreshToken,
                UserId = userId
            };

            _refreshTokenService.AddRefreshToken(refreshTokenModel);

            return (jwt, refreshToken);
        }
        #endregion
    }
}

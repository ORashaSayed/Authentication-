using AutoMapper;
using JWT.Business.Modules.UserModule.Interfaces;
using JWT.Business.Modules.UserModule.Requests;
using JWT.Business.Modules.UserModule.Responses;
using Serilog;
using System;
using System.Threading.Tasks;

namespace JWT.Business.Modules.UserModule
{
    internal class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public UserService(
            IUserRepository userRepository,
            ILogger logger,
            IMapper mapper
            )
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger.ForContext<UserService>();
        }

        public async Task<UserInfoResponse> GetUserInfoAsync(GetUserInfoRequest request)
        {
            try
            {
                var userToVerify = await _userRepository.GetUserByNameAsync(request.Username, includeRoles: true);

                if (await _userRepository.ValidateUserPasswordAsync(userToVerify.Id, request.Password))
                {
                    return new UserInfoResponse
                    {
                        Succeeded = true,
                        Id = userToVerify.Id,
                        Username = userToVerify.Username,
                        Email = userToVerify.Email,
                        Roles = userToVerify.Roles
                    };
                }
                else
                {
                    _logger.Error("Incorrect username or password.");
                    return new UserInfoResponse { Succeeded = false, Reason = $"Incorrect username or password." };
                }
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Exception has been thrown while getting user information.");
                return new UserInfoResponse { Succeeded = false, Reason = "An error has been occurred while getting user information." };
            }
        }

    }
}

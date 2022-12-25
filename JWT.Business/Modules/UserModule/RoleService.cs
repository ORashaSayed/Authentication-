using System;
using System.Threading.Tasks;
using AutoMapper;
using JWT.Business.Modules.UserModule.Interfaces;
using JWT.Business.Modules.UserModule.Responses;
using Serilog;

namespace JWT.Business.Modules.UserModule
{
    internal class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public RoleService(
             IRoleRepository roleRepository,
             IMapper mapper,
             ILogger logger
           )
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _logger = logger.ForContext<RoleService>();
        }
        public async Task<GetRolesResponse> GetRoles()
        {

            try
            {
                return new GetRolesResponse()
                {
                    Roles = await _roleRepository.GetRolesAsync()
                };
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Can't get Roles .");
                return new GetRolesResponse() { Reason = "Can't get Roles .", Succeeded = false };
            }
        }
    }
}

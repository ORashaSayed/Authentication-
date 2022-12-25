using System.ComponentModel.Composition;
using System.Linq;
using AutoMapper;
using JWT.Business.Modules.RefreshTokenModule.Models;
using JWT.Business.Modules.UserModule;
using JWT.DataAccess.Models;
using JWT.DependencyInjection;
using JWT.DependencyInjection.Registrars;
using Microsoft.AspNetCore.Identity;

namespace JWT.DataAccess
{
    [Export(typeof(IExport))]
    public class UserMappingProfile : Profile, IModule<IAutoMapperProfilesRegistrar>
    {
        public UserMappingProfile()
        {
            #region Identity
            CreateMap<IdentityError, Error>();
            CreateMap<IdentityResult, IdentityResultWrapper>();
            #endregion

            CreateMap<UserEntity, User>()
                .ForMember(dest => dest.Roles, opts => opts.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name)));

            // We are not copying back roles.. for now.
            CreateMap<User, UserEntity>();

            CreateMap<RefreshTokenEntity, RefreshToken>()
                .ReverseMap();
        }

        public void Initialize(IAutoMapperProfilesRegistrar registrar)
        {
            registrar.Add(typeof(UserMappingProfile));
        }
    }
}

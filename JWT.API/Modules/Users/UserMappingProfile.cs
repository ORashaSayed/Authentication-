using AutoMapper;
using JWT.API.Modules.Users.Models;
using JWT.API.Modules.Users.ViewModels;
using JWT.DependencyInjection;
using JWT.DependencyInjection.Registrars;
using System.ComponentModel.Composition;
using JWT.Business.Modules.UserModule.Responses;

namespace JWT.API.Modules.Users
{
    [Export(typeof(IExport))]
    public class UserMappingProfile : Profile, IModule<IAutoMapperProfilesRegistrar>
    {
        public UserMappingProfile()
        {
            CreateMap<UserInfoResponse, UserInfo>();
        }

        public void Initialize(IAutoMapperProfilesRegistrar registrar)
        {
            registrar.Add(typeof(UserMappingProfile));
        }
    }
}

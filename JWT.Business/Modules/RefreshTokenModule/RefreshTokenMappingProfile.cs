using System.ComponentModel.Composition;
using AutoMapper;
using JWT.Business.Modules.RefreshTokenModule.Models;
using JWT.Business.Modules.RefreshTokenModule.Requests;
using JWT.DependencyInjection;
using JWT.DependencyInjection.Registrars;

namespace JWT.Business.Modules.RefreshTokenModule
{
    [Export(typeof(IExport))]
    public class RefreshTokenMappingProfile : Profile, IModule<IAutoMapperProfilesRegistrar>
    {
        public RefreshTokenMappingProfile()
        {
            CreateMap<AddRefreshTokenRequest, RefreshToken>();
            CreateMap<DeleteRefreshTokenRequest, RefreshToken>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.RefreshTokenId));
        }

        public void Initialize(IAutoMapperProfilesRegistrar registrar)
        {
            registrar.Add(typeof(RefreshTokenMappingProfile));
        }
    }
}
using System.ComponentModel.Composition;
using JWT.DependencyInjection;
using JWT.DependencyInjection.Registrars;

namespace JWT.Business.Modules.RefreshTokenModule
{
    [Export(typeof(IExport))]
    public class RefreshTokenModuleRegistration : IModule<IModuleRegistrar>
    {
        public void Initialize(IModuleRegistrar registrar)
        {
            registrar.Register<IRefreshTokenService, RefreshTokenService>(Lifetime.Scoped);
        }
    }
}
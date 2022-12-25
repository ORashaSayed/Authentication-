using JWT.Business.Modules.UserModule.Interfaces;
using JWT.DependencyInjection;
using JWT.DependencyInjection.Registrars;
using System.ComponentModel.Composition;

namespace JWT.Business.Modules.UserModule
{
    [Export(typeof(IExport))]
    public class UserModuleRegistration : IModule<IModuleRegistrar>
    {
        public void Initialize(IModuleRegistrar registrar)
        {
            registrar.Register<IUserService, UserService>(Lifetime.Scoped);
        }
    }

}
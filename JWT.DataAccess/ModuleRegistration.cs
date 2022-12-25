using JWT.Business;
using JWT.Business.Modules.RefreshTokenModule;
using JWT.Business.Modules.UserModule.Interfaces;
using JWT.DataAccess.Models;
using JWT.DataAccess.Repositories;
using JWT.DependencyInjection;
using JWT.DependencyInjection.Registrars;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Composition;

namespace JWT.DataAccess
{
    [Export(typeof(IExport))]
    public class ModuleRegistration : IModule<IModuleRegistrar>, IModule<IIdentityRegistrar>, IModule<IDatabaseRegistrar>
    {
        public void Initialize(IModuleRegistrar registrar)
        {
            registrar.Register<IRefreshTokenRepository, RefreshTokenRepository>(Lifetime.Scoped);
            registrar.Register<IUnitOfWork, UnitOfWork>(Lifetime.Scoped);
            registrar.Register<IUserRepository, UserRepository>(Lifetime.Scoped);
            registrar.Register<IRoleRepository, RoleRepository>(Lifetime.Scoped);
            registrar.Register<IDatabaseMigrator, DatabaseMigrator>(Lifetime.Scoped);

        }

        public void Initialize(IDatabaseRegistrar databaseRegistrar)
        {
            databaseRegistrar.RegisterDbContext<ApplicationDbContext>((options, connectionString) =>
            {
                options.UseSqlServer(connectionString,
                    builder => builder.MigrationsAssembly(typeof(ModuleRegistration).Assembly.FullName));
            });
        }
        public void Initialize(IIdentityRegistrar identityRegistrar)
        {
            identityRegistrar.AddIdentity<UserEntity, RoleEntity, ApplicationDbContext>();
        }
    }
}

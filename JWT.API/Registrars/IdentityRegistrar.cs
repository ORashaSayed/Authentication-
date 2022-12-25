using JWT.DependencyInjection.Registrars;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JWT.Api.Registrars
{
    public class IdentityRegistrar : IIdentityRegistrar
    {
        private readonly IServiceCollection _services;
        public IdentityRegistrar(IServiceCollection services)
        {
            _services = services;
        }

        public void AddIdentity<TUser, TRole, TContext>()
            where TUser : class
            where TRole : class
            where TContext : DbContext
        {
            _services.AddIdentity<TUser, TRole>(opts =>
                {
                    opts.User.RequireUniqueEmail = Business.Modules.UserModule.UserOptions.RequireUniqueEmail;
                    opts.Password.RequiredLength = Business.Modules.UserModule.PasswordOptions.RequiredLength;
                    opts.Password.RequireDigit = Business.Modules.UserModule.PasswordOptions.RequireDigit;
                    opts.Password.RequireLowercase = Business.Modules.UserModule.PasswordOptions.RequireLowercase;
                    opts.Password.RequireUppercase = Business.Modules.UserModule.PasswordOptions.RequireUppercase;
                    opts.Password.RequireNonAlphanumeric = Business.Modules.UserModule.PasswordOptions.RequireNonAlphanumeric;
                })
                .AddEntityFrameworkStores<TContext>()
                .AddDefaultTokenProviders();
        }
    }
}

using System;
using System.Threading.Tasks;
using JWT.Business.Modules.UserModule.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JWT.Business.Modules.UserModule
{
    public static class UserModuleDataSeeding
    {
        public static async Task CheckandSeed(IServiceProvider services)
        {
            await CheckAndSeedRoles(services);
            await CheckAndSeedAdmin(services);
        }

        private static async Task<bool> CheckAndSeedRoles(IServiceProvider services)
        {
            bool created = false;
            using (var scope = services.CreateScope())
            {
                var roleRepository = scope.ServiceProvider.GetService<IRoleRepository>();
                var rolesExisted = await roleRepository.HasRolesAsync();
                if (!rolesExisted)
                {
                    await roleRepository.Add(Roles.Admin);
                    await roleRepository.Add(Roles.User);
                    created = true;
                }
            }
            return created;

        }

        private static async Task CheckAndSeedAdmin(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var configuration = serviceProvider.GetService<IConfiguration>();
            var adminUsername = configuration.GetValue<string>("AdminCredentials:UserName");
            var userRepository = scope.ServiceProvider.GetService<IUserRepository>();
            var admin = await userRepository.GetUserByNameAsync(adminUsername);
            if (admin == null)
            {
                var password = configuration.GetValue<string>("AdminCredentials:Password");
                var role = configuration.GetValue<string>("AdminCredentials:Role");
                admin = new User
                {
                    Username = adminUsername,
                    Email = configuration.GetValue<string>("AdminCredentials:Email"),
                    FirstName = configuration.GetValue<string>("AdminCredentials:FirstName"),
                    LastName = configuration.GetValue<string>("AdminCredentials:LastName"),
                    Region = configuration.GetValue<string>("AdminCredentials:Region")
                };

                await userRepository.AddUserAsync(admin, password);

                await userRepository.AddUserToRolesAsync(admin.Id, new[] { role });
            }
        }
    }
}

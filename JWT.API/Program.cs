using JWT.API.Helpers;
using JWT.Business.Modules.UserModule;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Threading.Tasks;
using JWT.DataAccess;

namespace JWT.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var webhHost = CreateHostBuilder(args).Build();
            MigrateDatabase(webhHost.Services);
            await UserModuleDataSeeding.CheckandSeed(webhHost.Services);
            await webhHost.RunAsync();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog(AzureLogBuilder.CreateLogger());

        private static void MigrateDatabase(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var migrator = scope.ServiceProvider.GetService<IDatabaseMigrator>();
                migrator.Migrate();
            }
        }
    }
}

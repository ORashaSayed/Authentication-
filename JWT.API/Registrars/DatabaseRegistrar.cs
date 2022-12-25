using JWT.DependencyInjection.Registrars;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace JWT.Api.Registrars
{
    internal class DatabaseRegistrar : IDatabaseRegistrar
    {
        private readonly IServiceCollection _services;
        private readonly string _connectionString;

        public DatabaseRegistrar(IServiceCollection services, string connectionString)
        {
            _services = services;
            _connectionString = connectionString;
        }

        public void RegisterDbContext<TContext>(Action<DbContextOptionsBuilder, string> options)
            where TContext : DbContext
        {
            _services.AddDbContext<TContext>(optionsBuilder => options(optionsBuilder, _connectionString));
        }
    }
}

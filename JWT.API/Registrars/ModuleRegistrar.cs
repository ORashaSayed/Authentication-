using JWT.DependencyInjection;
using JWT.DependencyInjection.Registrars;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace JWT.Api.Registrars
{
    internal class ModuleRegistrar : IModuleRegistrar
    {
        private readonly IServiceCollection _services;

        public ModuleRegistrar(IServiceCollection services)
        {
            _services = services;
        }

        public void Register<TFrom>(Func<IServiceProvider, TFrom> instanceCreator, Lifetime lifetime)
            where TFrom : class
        {
            switch (lifetime)
            {
                case Lifetime.Transient:
                    _services.AddTransient(instanceCreator);
                    break;
                case Lifetime.Scoped:
                    _services.AddScoped(instanceCreator);
                    break;
                case Lifetime.Singleton:
                    _services.AddSingleton(instanceCreator);
                    break;
                default:
                    throw new ArgumentException("Unsupported lifetime");
            }
        }

        public void Register<TFrom, TTo>(Lifetime lifetime)
            where TTo : class, TFrom
            where TFrom : class
        {
            Register(typeof(TFrom), typeof(TTo), lifetime);
        }


        public void Register(Type service, Type implementation, Lifetime lifetime)
        {
            switch (lifetime)
            {
                case Lifetime.Transient:
                    _services.AddTransient(service, implementation);
                    break;
                case Lifetime.Scoped:
                    _services.AddScoped(service, implementation);
                    break;
                case Lifetime.Singleton:
                    _services.AddSingleton(service, implementation);
                    break;
                default:
                    throw new ArgumentException("Unsupported lifetime");
            }
        }
    }
}

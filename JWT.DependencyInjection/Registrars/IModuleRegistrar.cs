using System;

namespace JWT.DependencyInjection.Registrars
{
    public interface IModuleRegistrar
    {
        void Register<TFrom, TTo>(Lifetime lifetime)
            where TTo : class, TFrom
            where TFrom : class;

        void Register<TImplementation>(Func<IServiceProvider, TImplementation> instanceCreator, Lifetime lifetime)
           where TImplementation : class;
    }
}

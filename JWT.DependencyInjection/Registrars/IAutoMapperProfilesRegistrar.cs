using System;

namespace JWT.DependencyInjection.Registrars
{
    public interface IAutoMapperProfilesRegistrar
    {
        void Add(Type type);
    }
}

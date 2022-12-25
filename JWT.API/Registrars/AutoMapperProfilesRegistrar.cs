using JWT.DependencyInjection.Registrars;
using System;
using System.Collections.Generic;

namespace JWT.Api.Registrars
{
    public class AutoMapperProfilesRegistrar : IAutoMapperProfilesRegistrar
    {
        public List<Type> ProfileTypes { get; } = new List<Type>();
        public void Add(Type type)
        {
            ProfileTypes.Add(type);
        }
    }
}

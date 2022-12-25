using System;
using Microsoft.Extensions.Caching.Memory;

namespace JWT.Caching
{
    public class MemoryCachingOptions : MemoryCacheEntryOptions
    {
        public void RegisterCallbackMethod(Action<object, object> postCallbackMethod)
        {
            this.RegisterPostEvictionCallback((key, value, reason, substate) =>
            {
                postCallbackMethod(key, value);
            });
        }
    }
}

using Microsoft.Extensions.Caching.Memory;

namespace JWT.Caching
{
    public class MemoryCaching : IMemoryCaching
    {
        private IMemoryCache _cache;
        private readonly MemoryCachingOptions _memoryCacheEntryOptions;

        public MemoryCaching(IMemoryCache cache, MemoryCachingOptions memoryCacheEntryOptions)
        {
            _cache = cache;
            _memoryCacheEntryOptions = memoryCacheEntryOptions;
        }

        public TItem Set<TItem>(object key, TItem item)
        {
            return Set<TItem>(key, item, _memoryCacheEntryOptions);
        }

        public TItem Set<TItem>(object key, TItem item, MemoryCachingOptions memoryCachingOptions)
        {
            return _cache.Set(key, item, memoryCachingOptions);
        }

        public bool TryGetValue<TItem>(object key, out TItem value)
        {
            return _cache.TryGetValue(key, out value);
        }
    }
}

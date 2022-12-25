namespace JWT.Caching
{
    public interface IMemoryCaching
    {
        TItem Set<TItem>(object key, TItem item);
        TItem Set<TItem>(object key, TItem item, MemoryCachingOptions memoryCachingOptions);
        bool TryGetValue<TItem>(object key, out TItem value);
    }
}

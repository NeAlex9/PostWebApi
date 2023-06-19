using Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Caching.InMemory
{
    internal class CachingService<T> : ICachingService<T>
    {
        private readonly IMemoryCache _cache;

        public CachingService(IMemoryCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public void Set(object id, DateTime validUntil, T obj)
        {
            _cache.Set(id, obj,
                new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(validUntil));
        }

        public bool TryGetValue(object id, out T obj)
        {
            return _cache.TryGetValue(id, out obj);
        }
    }
}

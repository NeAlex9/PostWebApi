using Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Caching.InMemory
{
    internal class CachingService : ICachingService
    {
        private readonly IMemoryCache _cache;

        public CachingService(IMemoryCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public void Set<T>(object id, DateTime validUntil, T obj)
        {
            _cache.Set(id, obj,
                new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(validUntil));
        }

        public bool TryGetValue<T>(object id, out T obj)
        {
            return _cache.TryGetValue(id, out obj);
        }
    }
}

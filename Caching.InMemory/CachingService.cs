using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Caching.InMemory
{
    internal class CachingService : ICachingService<Post>
    {
        private readonly IMemoryCache _cache;

        public CachingService(IMemoryCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public void Set(object id, DateTime validUntil, Post post)
        {
            _cache.Set(id, post,
                new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(validUntil));
        }

        public bool TryGetValue(object id, out Post post)
        {
            return _cache.TryGetValue(id, out post);
        }
    }
}

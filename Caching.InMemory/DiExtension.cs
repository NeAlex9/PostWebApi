using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Caching.InMemory
{
    public static class DiExtension
    {
        public static IServiceCollection AddCachingService(this IServiceCollection services)
        {
            services.AddMemoryCache();
            return services.AddSingleton<ICachingService<Post>, CachingService>();
        }
    }
}

using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.SqlLite
{
    public static class DiExtension
    {
        public static IServiceCollection AddRepository(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddScoped<IUnitOfWork, UnitOfWork>()
                           .AddScoped<ISqlConnectionFactory, SqlConnectionFactory>()
                           .Configure<SqlConnectionString>(configuration);
        }
    }
}

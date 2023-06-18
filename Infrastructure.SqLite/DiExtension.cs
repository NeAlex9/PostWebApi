using Domain.Interfaces;
using Infrastructure.SqlLite.Abstractions;
using Infrastructure.SqlLite.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.SqlLite
{
    public static class DiExtension
    {
        public static IServiceCollection AddSqLiteInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddTransient<IUnitOfWork, UnitOfWork>()
                           .AddTransient<ISqlConnectionFactory, SqlConnectionFactory>()
                           .Configure<SqlConnectionString>(configuration);
        }
    }
}

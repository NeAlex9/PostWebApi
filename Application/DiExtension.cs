using Application.Commands;
using Application.Queries;
using Application.Queries.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DiExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddMediatR(conf =>
            {
                conf.RegisterServicesFromAssemblies(
                    ApplicationCommandProvider.CommandsAssembly,
                    ApplicationQueryProvider.QueryAssembly);
            })
                .Configure<PostCachingOptions>(configuration);
        }
    }
}

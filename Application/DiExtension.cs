using Application.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DiExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services.AddMediatR(conf =>
            {
                conf.RegisterServicesFromAssemblies(
                    ApplicationCommandProvider.CommandsAssembly,
                    ApplicationQueryProvider.QueryAssembly);
            });
        }
    }
}

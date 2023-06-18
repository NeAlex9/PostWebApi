using Serilog;

namespace PostsApi.Helpers
{
    public static class LoggingExtension
    {
        public static void AddLogging(this WebApplicationBuilder builder)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                               .AddJsonFile($"appsettings.{environment}.json", optional: true)
                               .Build();

            Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(configuration)
                        .CreateLogger();

            builder.Host.UseSerilog();
        }
    }
}

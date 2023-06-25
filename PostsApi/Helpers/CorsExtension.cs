namespace PostsApi.Helpers
{
    public static class CorsExtension
    {
        public static IServiceCollection ConfigureCors(this IServiceCollection service) =>
            service.AddCors(options =>
                        options.AddPolicy("CORSPolicy", builder =>
                            builder
                               .AllowAnyHeader()
                               .AllowAnyMethod()
                               .AllowCredentials()
                                .WithOrigins(new[] { "http://localhost:8080" })));
    }
}

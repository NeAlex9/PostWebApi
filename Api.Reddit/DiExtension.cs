using Api.Reddit.Models;
using Api.Reddit.Services;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Reddit
{
    public static class DiExtension
    {
        public static void AddRedditApiService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiOptions>(configuration);

            var userAgent = configuration.GetValue<string>("UserAgent");
            services.AddHttpClient<IApiAuthenticator, RedditAuthenticator>(client =>
            {
                client.BaseAddress = new Uri("https://www.reddit.com/api/v1/");
                client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
            });
            services.AddHttpClient<IPostRetrivalClient, RedditRetrivalClient>(client =>
            {
                client.BaseAddress = new Uri("https://oauth.reddit.com/");
                client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
            });
        }
    }
}

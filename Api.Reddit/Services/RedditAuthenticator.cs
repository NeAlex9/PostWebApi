using Api.Reddit.Models;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Api.Reddit.Services
{
    internal class RedditAuthenticator : IApiAuthenticator
    {
        private readonly ApiOptions _redditOptions;
        private readonly UserCredentials _userCredentials;
        private readonly HttpClient _httpClient;

        public RedditAuthenticator(
            IOptions<ApiOptions> options,
            HttpClient httpClient,
            IOptions<UserCredentials> userCredentialsOptions)
        {
            _redditOptions = options.Value ?? throw new ArgumentNullException(nameof(options));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _userCredentials = userCredentialsOptions.Value ?? throw new ArgumentNullException(nameof(userCredentialsOptions));
        }

        public async Task<RedditToken> GetAccessToken(CancellationToken cancellationToken)
        {
            var apiKey = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_redditOptions.ApplicationId}:{_redditOptions.ApplicationSecret}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", apiKey);
            var values = new Dictionary<string, string>
            {
                { "grant_type",  _redditOptions.GrantType}
            };
            var content = new FormUrlEncodedContent(values);

            var postResponse = await _httpClient.PostAsync("access_token", content, cancellationToken);
            postResponse.EnsureSuccessStatusCode();
            var json = await postResponse.Content.ReadAsStringAsync();
            var accessToken = JsonSerializer.Deserialize<RedditToken>(json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });

            return accessToken;
        }
    }
}

using Api.Reddit.Models;
using Api.Reddit.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Api.Reddit
{
    internal class RedditRetrivalClient : IPostRetrivalClient
    {
        private static readonly Guid _accessTokenKey = Guid.NewGuid();

        private const int _postsCount = 15;
        private readonly HttpClient _httpClient;
        private readonly IApiAuthenticator _redditAuthenticator;
        private readonly ICachingService _cachingService;
        private readonly ILogger<RedditRetrivalClient> _logger;

        public RedditRetrivalClient(
            HttpClient httpClient,
            IApiAuthenticator authenticator,
            ICachingService cachingService,
            ILogger<RedditRetrivalClient> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _redditAuthenticator = authenticator ?? throw new ArgumentNullException(nameof(authenticator));
            _cachingService = cachingService ?? throw new ArgumentNullException(nameof(cachingService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Post>> GetPostsAsync(CancellationToken cancellationToken)
        {
            if (!_cachingService.TryGetValue(_accessTokenKey, out RedditToken accessToken))
            {
                accessToken = await GetAndSaveToken(cancellationToken);
            }
            else
            {
                _logger.LogInformation("Access token retrieved from cache");
            }

            SetHeaders(accessToken.Value);
            var popularResponse = await _httpClient.GetAsync($"r/funny/top?limit={_postsCount}");
            if (popularResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                accessToken = await GetAndSaveToken(cancellationToken);
                SetHeaders(accessToken.Value);
                popularResponse = await _httpClient.GetAsync($"r/funny/top?limit={_postsCount}");
                popularResponse.EnsureSuccessStatusCode();
            }

            var getResponseContent = await popularResponse.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<RedditApiResponse>(getResponseContent,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });
            var redditPosts = result.Data.Children;
            var posts = redditPosts.Select(redditPost => RedditPostMapper.Map(redditPost.Data))
                                   .ToList();
            return posts;
        }

        private void SetHeaders(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        private async Task<RedditToken> GetAndSaveToken(CancellationToken cancellationToken)
        {
            var accessToken = await _redditAuthenticator.GetAccessToken(cancellationToken);
            _logger.LogInformation("New access token retrieved from api");
            var validUntil = DateTime.UtcNow.AddSeconds(accessToken.ExpiredInSeconds);
            _cachingService.Set(_accessTokenKey, validUntil, accessToken);
            return accessToken;
        }
    }
}

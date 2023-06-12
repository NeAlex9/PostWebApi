using Domain.Entities;
using Domain.Interfaces;

namespace Api.Reddit
{
    internal class PostRetrivalClient : IPostRetrivalClient
    {
        private readonly HttpClient _httpClient;

        public PostRetrivalClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public Task<IEnumerable<Post>> GetPostsAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

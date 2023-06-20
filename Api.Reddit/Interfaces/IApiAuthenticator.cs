using Api.Reddit.Models;

namespace Domain.Interfaces
{
    internal interface IApiAuthenticator
    {
        Task<RedditToken> GetAccessToken(CancellationToken cancellationToken);
    }
}

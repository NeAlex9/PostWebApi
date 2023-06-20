using Api.Reddit.Models;
using Domain.Entities;

namespace Api.Reddit.Services
{
    internal static class RedditPostMapper
    {
        public static Post Map(RedditPostData redditPost)
        {
            var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds((long)redditPost.CreatedAt);
            var dateTimeUtc = dateTimeOffset.UtcDateTime;
            var id = Guid.NewGuid();
            var post = new Post(id, redditPost.Title, redditPost.AuthorName, redditPost.Score, dateTimeUtc);
            return post;
        }
    }
}

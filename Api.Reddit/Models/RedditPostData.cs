using System.Text.Json.Serialization;

namespace Api.Reddit.Models
{
    internal class RedditPostData
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("author")]
        public string AuthorName { get; set; }

        [JsonPropertyName("score")]
        public int Score { get; set; }

        [JsonPropertyName("created_utc")]
        public double CreatedAt { get; set; }
    }
}

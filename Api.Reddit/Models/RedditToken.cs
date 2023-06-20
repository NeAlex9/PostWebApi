using System.Text.Json.Serialization;

namespace Api.Reddit.Models
{
    internal class RedditToken
    {
        [JsonPropertyName("access_token")]
        public string Value { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiredInSeconds { get; set; }
    }
}

using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

[assembly: InternalsVisibleTo("UnitTests")]
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

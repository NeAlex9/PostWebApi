namespace Api.Reddit.Models
{
    public class ApiOptions
    {
        public string ApplicationId { get; set; }
        public string ApplicationSecret { get; set; }
        public string GrantType { get; set; }
        public string UserAgent { get; set; }
        public string UserName { get; set; }
    }
}

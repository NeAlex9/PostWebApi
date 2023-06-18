namespace Domain.Entities
{
    public class Post : IEntity
    {
        public Post(Guid id, string title, string authorName, int score, DateTime dateTime)
        {
            Id = id;
            Title = title ?? throw new ArgumentNullException(nameof(title));
            AuthorName = authorName ?? throw new ArgumentNullException(nameof(authorName));
            Score = score;
            CreatedAt = dateTime;
        }

        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string AuthorName { get; private set; }
        public int Score { get; private set; }
        public DateTime CreatedAt { get; private set; }
    }
}

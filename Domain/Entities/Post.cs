namespace Domain.Entities
{
    public class Post : IEntity
    {
        public Post(string title, string author, int score, DateTime dateTime)
        {
            Id = Guid.NewGuid();
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Author = author ?? throw new ArgumentNullException(nameof(author));
            Score = score;
            CreatedAt = dateTime;
        }

        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Author { get; private set; }
        public int Score { get; private set; }
        public DateTime CreatedAt { get; private set; }
    }
}

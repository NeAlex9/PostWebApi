using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICachingService
    {
        Task<Post> GetPostAsync(Guid id, CancellationToken cancellationToken);
        Task CreatePost(Post post, DateTime validUntil, CancellationToken cancellationToken);
    }
}

using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPostRepository
    {
        Task<Post> GetPostAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Post>> GetPostsAsync(CancellationToken cancellationToken);
        Task CreatePost(Post post, CancellationToken cancellationToken);
    }
}

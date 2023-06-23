using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPostRetrivalClient
    {
        Task<IEnumerable<Post>> GetPostsAsync(int postsCount, CancellationToken cancellationToken);
    }
}

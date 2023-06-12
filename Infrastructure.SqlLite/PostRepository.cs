using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.SqlLite
{
    internal class PostRepository : IPostRepository
    {
        public Task CreatePost(Post post, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Post> GetPostAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Post>> GetPostsAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

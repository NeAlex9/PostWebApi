using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Data.Sqlite;

namespace Infrastructure.SqlLite
{
    internal class PostRepository : IPostRepository
    {
        private readonly SqliteConnection _connection;
        private readonly SqliteTransaction _transaction;

        public PostRepository(SqliteConnection connection, SqliteTransaction transaction)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
        }

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
            return Task.FromResult(Enumerable.Empty<Post>());
        }
    }
}

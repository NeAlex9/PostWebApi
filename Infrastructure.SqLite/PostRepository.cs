using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.SqlLite.Abstractions;
using Microsoft.Data.Sqlite;

namespace Infrastructure.SqlLite
{
    internal class PostRepository : BaseRepository, IPostRepository
    {
        public PostRepository(SqliteConnection connection, SqliteTransaction transaction)
            : base(connection, transaction)
        {
        }

        public async Task CreatePost(Post post, CancellationToken cancellationToken)
        {
            const string commandText = @"INSERT INTO Posts(Id, Title, AuthorName, Score, CreatedAt)
                                         VALUES (@id, @title, @authorName, @score, @createdAt)";
            using var command = CreateCommand();
            command.CommandText = commandText;
            command.Parameters.AddWithValue("@id", post.Id);
            command.Parameters.AddWithValue("@title", post.Title);
            command.Parameters.AddWithValue("@authorName", post.AuthorName);
            command.Parameters.AddWithValue("@score", post.Score);
            command.Parameters.AddWithValue("@createdAt", post.CreatedAt);

            await command.ExecuteNonQueryAsync();
        }

        public async Task CreatePosts(IEnumerable<Post> posts, CancellationToken cancellationToken)
        {
            const string commandText = @"INSERT INTO Posts(Id, Title, AuthorName, Score, CreatedAt)
                                         VALUES (@id, @title, @authorName, @score, @createdAt)";
            using var command = CreateCommand();
            command.CommandText = commandText;

            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            command.Parameters.Add(idParameter);

            var titleParameter = command.CreateParameter();
            titleParameter.ParameterName = "@title";
            command.Parameters.Add(titleParameter);

            var authorNameParameter = command.CreateParameter();
            authorNameParameter.ParameterName = "@authorName";
            command.Parameters.Add(authorNameParameter);

            var scoreParameter = command.CreateParameter();
            scoreParameter.ParameterName = "@score";
            command.Parameters.Add(scoreParameter);

            var createdAtParameter = command.CreateParameter();
            createdAtParameter.ParameterName = "@createdAt";
            command.Parameters.Add(createdAtParameter);
            var tasks = new List<Task>();
            foreach (var post in posts)
            {
                var id = Guid.NewGuid();
                idParameter.Value = id;
                titleParameter.Value = post.Title;
                authorNameParameter.Value = post.AuthorName;
                scoreParameter.Value = post.Score;
                createdAtParameter.Value = post.CreatedAt;
                var task = command.ExecuteNonQueryAsync(cancellationToken);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
        }

        public async Task<Post?> GetPostAsync(Guid id, CancellationToken cancellationToken)
        {
            const string queryText = @"SELECT * FROM Posts
                                       WHERE id = @id";
            using var query = CreateCommand();
            query.CommandText = queryText;
            query.Parameters.AddWithValue("@id", id);
            using var reader = await query.ExecuteReaderAsync(cancellationToken);
            if (reader.HasRows)
            {
                reader.Read();
                var post = RetrievePost(reader);
                return post;
            }

            return null;
        }

        public async Task<IEnumerable<Post>> GetPostsAsync(CancellationToken cancellationToken)
        {
            const string queryText = @"SELECT * FROM Posts";
            using var query = CreateCommand();
            query.CommandText = queryText;
            using var reader = await query.ExecuteReaderAsync(cancellationToken);
            var posts = new List<Post>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var post = RetrievePost(reader);
                    posts.Add(post);
                }

                reader.NextResult();
            }

            return posts;
        }

        private Post RetrievePost(SqliteDataReader reader)
        {
            var id = reader.GetGuid(0);
            var title = reader.GetString(1);
            var authorName = reader.GetString(2);
            var score = reader.GetInt32(3);
            var createdAt = reader.GetDateTime(4);
            return new Post(id, title, authorName, score, createdAt);
        }
    }
}

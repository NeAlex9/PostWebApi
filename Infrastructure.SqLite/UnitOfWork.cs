using Domain.Interfaces;
using Infrastructure.SqlLite.Abstractions;
using Microsoft.Data.Sqlite;

namespace Infrastructure.SqlLite
{
    internal class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly SqliteTransaction _transaction;
       
        public UnitOfWork(ISqlConnectionFactory factory) 
        {
            _connection = factory.Create();
            _connection.Open();
            _transaction = _connection.BeginTransaction();
            PostRepository = new PostRepository(_connection, _transaction);
        }

        public IPostRepository PostRepository { get; }

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
            }

            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
            }
        }

        public async Task SaveChanges(CancellationToken cancellationToken)
        {
            await _transaction.CommitAsync(cancellationToken);
        }
    }
}

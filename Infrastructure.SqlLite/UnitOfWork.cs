using Domain.Interfaces;
using Infrastructure.SqlLite.Interfaces;
using Microsoft.Data.Sqlite;

namespace Infrastructure.SqlLite
{
    internal class UnitOfWork : IUnitOfWork
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

        public async Task SaveChanges()
        {
            await _transaction.CommitAsync();
        }
    }
}

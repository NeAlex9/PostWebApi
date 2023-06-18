using Microsoft.Data.Sqlite;

namespace Infrastructure.SqlLite.Abstractions
{
    public abstract class BaseRepository
    {
        protected SqliteConnection _connection;
        protected SqliteTransaction _transaction;

        protected BaseRepository(SqliteConnection connection, SqliteTransaction transaction)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection)); ;
            _transaction = transaction ?? throw new ArgumentNullException(nameof(transaction)); ;
        }

        protected SqliteCommand CreateCommand()
        {
            var command = _connection.CreateCommand();
            command.Transaction = _transaction;
            return command;
        }
    }
}

using Infrastructure.SqlLite.Interfaces;
using Infrastructure.SqlLite.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace Infrastructure.SqlLite
{
    internal class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(IOptions<SqlConnectionString> options)
        {
            _connectionString = options.Value.ConnectionString;
            _connectionString = _connectionString ?? throw new ArgumentNullException(nameof(options));
        }

        public SqliteConnection Create()
        {
            var connection = new SqliteConnection(_connectionString);
            return connection;
        }
    }
}

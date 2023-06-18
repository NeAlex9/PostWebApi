using Microsoft.Data.Sqlite;

namespace Infrastructure.SqlLite.Abstractions
{
    internal interface ISqlConnectionFactory
    {
        SqliteConnection Create();
    }
}

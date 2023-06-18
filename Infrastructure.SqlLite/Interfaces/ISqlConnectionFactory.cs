using Microsoft.Data.Sqlite;

namespace Infrastructure.SqlLite.Interfaces
{
    internal interface ISqlConnectionFactory
    {
        SqliteConnection Create();
    }
}

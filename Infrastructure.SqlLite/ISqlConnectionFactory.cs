using Microsoft.Data.Sqlite;

namespace Infrastructure.SqlLite
{
    internal interface ISqlConnectionFactory
    {
        SqliteConnection Create();
    }
}

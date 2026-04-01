using Microsoft.Data.Sqlite;

namespace TaskManager.Infra.Database.Interfaces
{
    public interface ISqliteConnectionFactory
    {        
        Task<SqliteConnection> CreateOpenConnectionAsync();
        void EnsureDatabase();
    }
}

using Microsoft.Data.Sqlite;

namespace TaskManager.Infra.Database.Interfaces
{
    public interface ISqliteConnectionFactory
    {
        string GetConnectionString { get; }
        Task<SqliteConnection> CreateOpenConnectionAsync();
        void EnsureDatabase();
    }
}

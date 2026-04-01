using Microsoft.Data.Sqlite;
using TaskManager.Infra.Database.Interfaces;

namespace TaskManager.Infra.Database
{
    public class SqliteConnectionFactory : ISqliteConnectionFactory
    {        
        public readonly string _connectionString;

        public SqliteConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Sqlite") ?? "Data Source=tasks.db";
        }        
        public async Task<SqliteConnection> CreateOpenConnectionAsync()
        {
            var conn = new SqliteConnection(_connectionString);
            await conn.OpenAsync();
            return conn;
        }

        public void EnsureDatabase()
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = 
                @"
                CREATE TABLE IF NOT EXISTS Tarefas (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Titulo TEXT NOT NULL,
                    Descricao TEXT,
                    Status INTEGER NOT NULL,
                    DataCriacao TEXT,
                    DataAlteracao TEXT
                );";
            cmd.ExecuteNonQuery();
        }
    }
}

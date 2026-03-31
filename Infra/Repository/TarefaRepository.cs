using Dapper;
using Microsoft.Data.Sqlite;
using TaskManager.Domain;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Infra.Repository
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly string _connectionString;

        public TarefaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Sqlite") ?? "Data Source=tasks.db";
        }

        private async Task<SqliteConnection> OpenConnectionAsync()
        {
            var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        public async Task<IEnumerable<Tarefa>> GetAll()
        {
            using var conn = await OpenConnectionAsync();
            var query = "SELECT Id, Titulo, Descricao, Status, DataCriacao, DataAlteracao FROM Tarefas";
            return await conn.QueryAsync<Tarefa>(query);
        }
        public async Task<Tarefa?> GetById(int id)
        {
            using var conn = await OpenConnectionAsync();
            var query = "SELECT Id, Titulo, Descricao, Status, DataCriacao, DataAlteracao FROM Tarefas WHERE Id = @Id";
            return await conn.QueryFirstOrDefaultAsync<Tarefa>(new CommandDefinition(query, new { Id = id }));
        }

        public async Task<Tarefa> Create(Tarefa entity)
        {
            using var conn = await OpenConnectionAsync();
            var query = @"INSERT INTO Tarefas (Titulo, Descricao, Status, DataCriacao, DataAlteracao)
                        VALUES (@Titulo, @Descricao, @Status, @DataCriacao, @DataAlteracao);
                        SELECT last_insert_rowid();";
            var id = await conn.ExecuteScalarAsync<long>(query, entity);
            // Cria uma nova instância de Tarefa para retornar, já que TarefaDto não possui Id
            return new Tarefa
            {
                Id = (int)id,
                Titulo = entity.Titulo,
                Descricao = entity.Descricao,
                Status = entity.Status,
                DataCriacao = entity.DataCriacao,
                DataAlteracao = entity.DataAlteracao
            };
        }

    }
}


























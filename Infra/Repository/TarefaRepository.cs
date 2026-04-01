using Dapper;
using TaskManager.Domain;
using TaskManager.Domain.Interfaces;
using TaskManager.Infra.Database.Interfaces;

namespace TaskManager.Infra.Repository
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly ISqliteConnectionFactory _connectionFactory;

        public TarefaRepository(ISqliteConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Tarefa>> GetAll()
        {
            using var conn = await _connectionFactory.CreateOpenConnectionAsync();
            var query = @"SELECT Id, Titulo, Descricao, Status, DataCriacao, DataAlteracao FROM Tarefas";
            return await conn.QueryAsync<Tarefa>(query);
        }
        public async Task<Tarefa?> GetById(int id)
        {
            using var conn = await _connectionFactory.CreateOpenConnectionAsync();
            var query = @"SELECT Id, Titulo, Descricao, Status, DataCriacao, DataAlteracao FROM Tarefas WHERE Id = @Id";
            return await conn.QueryFirstOrDefaultAsync<Tarefa>(new CommandDefinition(query, new { Id = id }));
        }

        public async Task<Tarefa> Create(Tarefa entity)
        {
            using var conn = await _connectionFactory.CreateOpenConnectionAsync();
            var query = @"INSERT INTO Tarefas (Titulo, Descricao, Status, DataCriacao, DataAlteracao)
                        VALUES (@Titulo, @Descricao, @Status, @DataCriacao, @DataAlteracao);
                        SELECT last_insert_rowid();";
            var id = await conn.ExecuteScalarAsync<long>(query, entity);
            entity.Id = (int)id;
            return entity;
        }

        public async Task<bool> Delete(int id)
        {
            using var conn = await _connectionFactory.CreateOpenConnectionAsync();
            var query = @"DELETE FROM Tarefas WHERE Id = @Id";
            var rows = await conn.ExecuteAsync(query, new { Id = id });
            return rows > 0;
        }

    }
}


























namespace TaskManager.Domain.Interfaces
{
    public interface ITarefaRepository
    {
        Task<IEnumerable<Tarefa>> GetAll();
        Task<Tarefa?> GetById(int id);
        Task<Tarefa> Create(Tarefa entity);
    }
}

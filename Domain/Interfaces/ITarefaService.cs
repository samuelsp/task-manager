namespace TaskManager.Domain.Interfaces
{
    public interface ITarefaService
    {
        Task<IEnumerable<Tarefa>> GetAll();
        Task<Tarefa?> GetById(int id);
        Task<Tarefa> Create(TarefaDto tarefa);
    }
}

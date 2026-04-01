using TaskManager.Domain;
using TaskManager.Domain.Enum;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Service
{
    public class TarefaService : ITarefaService
    {
        private readonly ITarefaRepository _tarefaRepository;
        public TarefaService(ITarefaRepository tarefaRepository)
        {
            _tarefaRepository = tarefaRepository;
        }
        public async Task<IEnumerable<Tarefa>> GetAll() => await _tarefaRepository.GetAll();
        public async Task<Tarefa?> GetById(int id) => await _tarefaRepository.GetById(id);
        public async Task<Tarefa> Create(TarefaDto tarefa)
        {
            var entity = new Tarefa
            {
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                Status = Status.Pendente,
                DataCriacao = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow
            };
            return await _tarefaRepository.Create(entity);
        }
    }
}

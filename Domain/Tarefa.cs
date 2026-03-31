using TaskManager.Domain.Enum;

namespace TaskManager.Domain
{
    public class Tarefa
    {
        public int Id { get; set; }
        public DateTime DataCriacao { get; set; } 
        public DateTime DataAlteracao { get; set; }
        public string? Titulo { get; set; } 
        public string? Descricao { get; set; } 
        public Status Status { get; set; }
    }

    public class TarefaDto
    {
        public string? Titulo { get; set; } 
        public string? Descricao { get; set; }        
    }
}

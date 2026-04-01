using Microsoft.AspNetCore.Mvc;
using TaskManager.Domain;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Web.Api.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TarefaController : Controller
    {
        private readonly ITarefaService _tarefaService;

        public TarefaController(ITarefaService tarefaService)
        {
            _tarefaService = tarefaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tarefa>>> GetAll()
        {
            var tarefas = await _tarefaService.GetAll();
            return Ok(tarefas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tarefa>> GetById(int id)
        {
            var tarefa = await _tarefaService.GetById(id);
            if (tarefa == null) return NotFound();
            return Ok(tarefa);
        }

        [HttpPost]
        public async Task<ActionResult<Tarefa>> Create([FromBody] TarefaDto tarefa)
        {
            if (tarefa == null)
            {
                return BadRequest();
            }
            var created = await _tarefaService.Create(tarefa);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            var success = await _tarefaService.Delete(id);
            if(!success) return NotFound();
            return NoContent();
        }
    }
}

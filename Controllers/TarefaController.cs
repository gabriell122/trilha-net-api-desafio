using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            // Implemenado
            var retorno = _context.Tarefas.Find(id);
            if( retorno == null)
                return NotFound();

            return Ok(retorno);
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            // Implemenado
            var retorno = _context.Tarefas.AsQueryable();

            return Ok(retorno);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            // Implemenado
            if (string.IsNullOrEmpty(titulo))
                return BadRequest(new {Erro = "O titulo não pode estar vazio"});
            var retorno = _context.Tarefas.Where(x=>x.Titulo == titulo);
            if ( retorno == null)
                return NotFound();

            return Ok(retorno);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            if(data == null)
                return BadRequest();
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            if(tarefa == null)
                return NotFound();
            return Ok(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            // Implemenado
            if(status == null)
                return BadRequest( new { Erro = "O campo status não pode ser vazio"});
            var tarefa = _context.Tarefas.Where(x => x.Status == status);
            if(tarefa == null)
                return NotFound();

            return Ok(tarefa);
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });
            // Implemenado
            
            if (string.IsNullOrEmpty(tarefa.Titulo) || string.IsNullOrEmpty(tarefa.Descricao) || tarefa.Status == null)
                return BadRequest( new { Erro = "Dados estão vazios verifique os dados"});
            _context.Tarefa.Add(tarefa);
            _context.Tarefa.SaveChanges();

            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // Implemenado
            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;

            _context.Tarefa.Update(tarefaBanco);
            _context.Tarefa.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();
            
            // Implemenado
            _context.Tarefa.Remove(tarefaBanco);
            _context.Tarefa.SaveChanges();

            return NoContent();
        }
    }
}

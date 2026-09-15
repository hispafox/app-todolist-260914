using AppTodoList.Api.Services;
using AppTodoList.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppTodoList.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TareasController : ControllerBase
{
    private readonly ITodoService _todoService;

    public TareasController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetAll()
    {
        var tareas = await _todoService.ObtenerTodosAsync();
        return Ok(tareas);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TodoItem>> GetById(int id)
    {
        var tarea = await _todoService.ObtenerPorIdAsync(id);
        if (tarea is null)
        {
            return NotFound();
        }

        return Ok(tarea);
    }

    [HttpPost]
    public async Task<ActionResult<TodoItem>> Create(TodoItem todoItem)
    {
        try
        {
            var creada = await _todoService.CrearAsync(todoItem);
            return CreatedAtAction(nameof(GetById), new { id = creada.Id }, creada);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TodoItem>> Update(int id, TodoItem todoItem)
    {
        try
        {
            var actualizada = await _todoService.ActualizarAsync(id, todoItem);
            if (actualizada is null)
            {
                return NotFound();
            }

            return Ok(actualizada);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await _todoService.EliminarAsync(id);
        if (!eliminado)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost("{id:int}/completar")]
    public async Task<ActionResult<TodoItem>> Completar(int id)
    {
        var tarea = await _todoService.CompletarAsync(id);
        if (tarea is null)
        {
            return NotFound();
        }

        return Ok(tarea);
    }
}

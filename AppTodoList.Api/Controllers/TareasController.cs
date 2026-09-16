using AppTodoList.Api.Dtos;
using AppTodoList.Api.Services;
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
    public async Task<ActionResult<IEnumerable<TareaDto>>> GetAll()
    {
        var tareas = await _todoService.ObtenerTodosAsync();
        return Ok(tareas);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TareaDto>> GetById(int id)
    {
        var tarea = await _todoService.ObtenerPorIdAsync(id);
        if (tarea is null)
        {
            return NotFound();
        }

        return Ok(tarea);
    }

    [HttpPost]
    public async Task<ActionResult<TareaDto>> Create([FromBody] GuardarTareaDto dto)
    {
        try
        {
            var creada = await _todoService.CrearAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = creada.Id }, creada);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TareaDto>> Update(int id, [FromBody] GuardarTareaDto dto)
    {
        try
        {
            var actualizada = await _todoService.ActualizarAsync(id, dto);
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
    public async Task<ActionResult<TareaDto>> Completar(int id)
    {
        var tarea = await _todoService.CompletarAsync(id);
        if (tarea is null)
        {
            return NotFound();
        }

        return Ok(tarea);
    }
}

using AppTodoList.Api.Services;
using AppTodoList.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppTodoList.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlantillasController : ControllerBase
{
    private readonly IPlantillaService _plantillaService;

    public PlantillasController(IPlantillaService plantillaService)
    {
        _plantillaService = plantillaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlantillaTarea>>> GetAll()
    {
        var plantillas = await _plantillaService.ObtenerTodasAsync();
        return Ok(plantillas);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PlantillaTarea>> GetById(int id)
    {
        var plantilla = await _plantillaService.ObtenerPorIdAsync(id);
        if (plantilla is null)
        {
            return NotFound();
        }

        return Ok(plantilla);
    }

    [HttpPost]
    public async Task<ActionResult<PlantillaTarea>> Create(PlantillaTarea plantilla)
    {
        try
        {
            var creada = await _plantillaService.CrearAsync(plantilla);
            return CreatedAtAction(nameof(GetById), new { id = creada.Id }, creada);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PlantillaTarea>> Update(int id, PlantillaTarea plantilla)
    {
        try
        {
            var actualizada = await _plantillaService.ActualizarAsync(id, plantilla);
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
        var eliminado = await _plantillaService.EliminarAsync(id);
        if (!eliminado)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost("{id:int}/instanciar")]
    public async Task<ActionResult<TodoItem>> Instanciar(int id)
    {
        var tarea = await _plantillaService.InstanciarAsync(id);
        if (tarea is null)
        {
            return NotFound();
        }

        return CreatedAtAction(nameof(TareasController.GetById), "Tareas", new { id = tarea.Id }, tarea);
    }
}

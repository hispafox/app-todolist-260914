using AppTodoList.Api.Dtos;
using AppTodoList.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppTodoList.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriasController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetAll()
    {
        var categorias = await _categoriaService.ObtenerTodosAsync();
        return Ok(categorias);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoriaDto>> GetById(int id)
    {
        var categoria = await _categoriaService.ObtenerPorIdAsync(id);
        if (categoria is null)
        {
            return NotFound();
        }

        return Ok(categoria);
    }

    [HttpPost]
    public async Task<ActionResult<CategoriaDto>> Create([FromBody] GuardarCategoriaDto dto)
    {
        try
        {
            var creada = await _categoriaService.CrearAsync(dto);
            return Created($"/api/categorias/{creada.Id}", creada);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CategoriaDto>> Update(int id, [FromBody] GuardarCategoriaDto dto)
    {
        try
        {
            var actualizada = await _categoriaService.ActualizarAsync(id, dto);
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
}

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
}

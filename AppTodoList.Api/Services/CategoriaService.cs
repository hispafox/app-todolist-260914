using AppTodoList.Api.Dtos;
using AppTodoList.Api.LogicaNegocio;
using AppTodoList.Models;

namespace AppTodoList.Api.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaLogica _categoriaLogica;

    public CategoriaService(ICategoriaLogica categoriaLogica)
    {
        _categoriaLogica = categoriaLogica;
    }

    public async Task<IEnumerable<CategoriaDto>> ObtenerTodosAsync()
    {
        var categorias = await _categoriaLogica.ObtenerTodosAsync();
        return categorias.Select(MapearADto);
    }

    public async Task<CategoriaDto?> ObtenerPorIdAsync(int id)
    {
        var categoria = await _categoriaLogica.ObtenerPorIdAsync(id);
        return categoria is null ? null : MapearADto(categoria);
    }

    public async Task<CategoriaDto> CrearAsync(GuardarCategoriaDto dto)
    {
        var entidad = new Categoria
        {
            Nombre = dto.Nombre,
            Color = dto.Color
        };

        var creada = await _categoriaLogica.CrearAsync(entidad);
        return MapearADto(creada ?? entidad);
    }

    public async Task<CategoriaDto?> ActualizarAsync(int id, GuardarCategoriaDto dto)
    {
        var entidad = new Categoria
        {
            Nombre = dto.Nombre,
            Color = dto.Color
        };

        var actualizada = await _categoriaLogica.ActualizarAsync(id, entidad);
        return actualizada is null ? null : MapearADto(actualizada);
    }

    public async Task<bool> EliminarAsync(int id)
        => await _categoriaLogica.EliminarAsync(id);

    private static CategoriaDto MapearADto(Categoria categoria) => new()
    {
        Id = categoria.Id,
        Nombre = categoria.Nombre,
        Color = categoria.Color
    };
}

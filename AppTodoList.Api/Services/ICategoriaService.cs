using AppTodoList.Api.Dtos;

namespace AppTodoList.Api.Services;

public interface ICategoriaService
{
    Task<IEnumerable<CategoriaDto>> ObtenerTodosAsync();
    Task<CategoriaDto?> ObtenerPorIdAsync(int id);
    Task<CategoriaDto> CrearAsync(GuardarCategoriaDto dto);
    Task<CategoriaDto?> ActualizarAsync(int id, CategoriaDto categoria);
    Task<bool> EliminarAsync(int id);
}

using AppTodoList.Api.Dtos;

namespace AppTodoList.Api.Services;

public interface ITodoService
{
    Task<IEnumerable<TareaDto>> ObtenerTodosAsync();
    Task<TareaDto?> ObtenerPorIdAsync(int id);
    Task<TareaDto> CrearAsync(GuardarTareaDto dto);
    Task<TareaDto?> ActualizarAsync(int id, GuardarTareaDto dto);
    Task<bool> EliminarAsync(int id);
    Task<TareaDto?> CompletarAsync(int id);
}

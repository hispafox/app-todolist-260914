using AppTodoList.Models;

namespace AppTodoList.Api.Services;

public interface ITodoService
{
    Task<IEnumerable<TodoItem>> ObtenerTodosAsync();
    Task<TodoItem?> ObtenerPorIdAsync(int id);
    Task<TodoItem> CrearAsync(TodoItem todoItem);
    Task<TodoItem?> ActualizarAsync(int id, TodoItem todoItem);
    Task<bool> EliminarAsync(int id);
    Task<TodoItem?> CompletarAsync(int id);
}

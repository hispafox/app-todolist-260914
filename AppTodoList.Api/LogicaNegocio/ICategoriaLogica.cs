using AppTodoList.Models;

namespace AppTodoList.Api.LogicaNegocio;

public interface ICategoriaLogica
{
    Task<IEnumerable<Categoria>> ObtenerTodosAsync();
    Task<Categoria?> ObtenerPorIdAsync(int id);
    Task<Categoria?> CrearAsync(Categoria categoria);
    Task<Categoria?> ActualizarAsync(int id, Categoria categoria);
    Task<bool> EliminarAsync(int id);
}

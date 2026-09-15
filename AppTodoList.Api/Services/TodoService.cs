using AppTodoList.Api.Data;
using AppTodoList.Models;
using Microsoft.EntityFrameworkCore;

namespace AppTodoList.Api.Services;

public class TodoService : ITodoService
{
    private readonly AppDbContext _context;

    public TodoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TodoItem>> ObtenerTodosAsync()
    {
        return await _context.TodoItems
            .AsNoTracking()
            .Include(todo => todo.Plantilla)
            .Include(todo => todo.Categoria)
            .Include(todo => todo.Persona)
            .OrderByDescending(todo => todo.CreatedAt)
            .ToListAsync();
    }

    public async Task<TodoItem?> ObtenerPorIdAsync(int id)
    {
        return await _context.TodoItems
            .AsNoTracking()
            .Include(todo => todo.Plantilla)
            .Include(todo => todo.Categoria)
            .Include(todo => todo.Persona)
            .FirstOrDefaultAsync(todo => todo.Id == id);
    }

    public async Task<TodoItem> CrearAsync(TodoItem todoItem)
    {
        ValidarTodoItem(todoItem);

        if (todoItem.CreatedAt == default)
        {
            todoItem.CreatedAt = DateTime.UtcNow;
        }

        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        return todoItem;
    }

    public async Task<TodoItem?> ActualizarAsync(int id, TodoItem todoItem)
    {
        var tareaExistente = await _context.TodoItems.FirstOrDefaultAsync(todo => todo.Id == id);
        if (tareaExistente is null)
        {
            return null;
        }

        ValidarTodoItem(todoItem);

        tareaExistente.Title = todoItem.Title;
        tareaExistente.IsCompleted = todoItem.IsCompleted;
        tareaExistente.EsRepetitiva = todoItem.EsRepetitiva;
        tareaExistente.Recurrencia = todoItem.Recurrencia;
        tareaExistente.ProximaFecha = todoItem.ProximaFecha;
        tareaExistente.PlantillaId = todoItem.PlantillaId;
        tareaExistente.CategoriaId = todoItem.CategoriaId;
        tareaExistente.PersonaId = todoItem.PersonaId;

        await _context.SaveChangesAsync();

        return await ObtenerPorIdAsync(id);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var tarea = await _context.TodoItems.FirstOrDefaultAsync(todo => todo.Id == id);
        if (tarea is null)
        {
            return false;
        }

        _context.TodoItems.Remove(tarea);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<TodoItem?> CompletarAsync(int id)
    {
        var tarea = await _context.TodoItems
            .Include(todo => todo.Plantilla)
            .Include(todo => todo.Categoria)
            .Include(todo => todo.Persona)
            .FirstOrDefaultAsync(todo => todo.Id == id);

        if (tarea is null)
        {
            return null;
        }

        tarea.IsCompleted = true;

        if (tarea.EsRepetitiva && tarea.Recurrencia.HasValue)
        {
            var proximaFecha = CalcularProximaFecha(tarea.Recurrencia.Value, tarea.ProximaFecha ?? tarea.CreatedAt);
            tarea.ProximaFecha = proximaFecha;

            var siguienteOcurrencia = new TodoItem
            {
                Title = tarea.Title,
                IsCompleted = false,
                CreatedAt = proximaFecha,
                EsRepetitiva = tarea.EsRepetitiva,
                Recurrencia = tarea.Recurrencia,
                ProximaFecha = CalcularProximaFecha(tarea.Recurrencia.Value, proximaFecha),
                PlantillaId = tarea.PlantillaId,
                CategoriaId = tarea.CategoriaId,
                PersonaId = tarea.PersonaId
            };

            _context.TodoItems.Add(siguienteOcurrencia);
        }

        await _context.SaveChangesAsync();

        return await ObtenerPorIdAsync(tarea.Id);
    }

    private static void ValidarTodoItem(TodoItem todoItem)
    {
        if (string.IsNullOrWhiteSpace(todoItem.Title))
        {
            throw new ArgumentException("El título es obligatorio.");
        }

        if (todoItem.EsRepetitiva && !todoItem.Recurrencia.HasValue)
        {
            throw new ArgumentException("La recurrencia es obligatoria para tareas repetitivas.");
        }
    }

    private static DateTime CalcularProximaFecha(TipoRecurrencia recurrencia, DateTime fechaBase)
    {
        return recurrencia switch
        {
            TipoRecurrencia.Diaria => fechaBase.AddDays(1),
            TipoRecurrencia.Semanal => fechaBase.AddDays(7),
            TipoRecurrencia.Mensual => fechaBase.AddMonths(1),
            _ => fechaBase.AddDays(1)
        };
    }
}

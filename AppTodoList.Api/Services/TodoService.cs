using AppTodoList.Api.Data;
using AppTodoList.Api.Dtos;
using AppTodoList.Api.LogicaNegocio;
using AppTodoList.Models;
using Microsoft.EntityFrameworkCore;

namespace AppTodoList.Api.Services;

public class TodoService : ITodoService
{
    private readonly AppDbContext _context;
    private readonly ICategoriaLogica _categoriaLogica;

    public TodoService(AppDbContext context, ICategoriaLogica categoriaLogica)
    {
        _context = context;
        _categoriaLogica = categoriaLogica;
    }

    public async Task<IEnumerable<TareaDto>> ObtenerTodosAsync()
    {
        var tareas = await _context.TodoItems
            .AsNoTracking()
            .Include(todo => todo.Plantilla)
            .Include(todo => todo.Categoria)
            .Include(todo => todo.Persona)
            .OrderByDescending(todo => todo.CreatedAt)
            .ToListAsync();

        return tareas.Select(MapearADto);
    }

    public async Task<TareaDto?> ObtenerPorIdAsync(int id)
    {
        var tarea = await _context.TodoItems
            .AsNoTracking()
            .Include(todo => todo.Plantilla)
            .Include(todo => todo.Categoria)
            .Include(todo => todo.Persona)
            .FirstOrDefaultAsync(todo => todo.Id == id);

        return tarea is null ? null : MapearADto(tarea);
    }

    public async Task<TareaDto> CrearAsync(GuardarTareaDto dto)
    {
        ValidarTodo(dto);
        await ValidarCategoriaAsync(dto.CategoriaId);

        var tarea = new TodoItem
        {
            Title = dto.Titulo,
            IsCompleted = dto.Completada,
            CreatedAt = DateTime.UtcNow,
            EsRepetitiva = dto.EsRepetitiva,
            Recurrencia = dto.Recurrencia,
            ProximaFecha = dto.EsRepetitiva && dto.Recurrencia.HasValue ? CalcularProximaFecha(dto.Recurrencia.Value, DateTime.UtcNow) : null,
            PlantillaId = null,
            CategoriaId = dto.CategoriaId,
            PersonaId = null
        };

        _context.TodoItems.Add(tarea);
        await _context.SaveChangesAsync();

        return await ObtenerPorIdAsync(tarea.Id) ?? MapearADto(tarea);
    }

    public async Task<TareaDto?> ActualizarAsync(int id, GuardarTareaDto dto)
    {
        var tareaExistente = await _context.TodoItems.FirstOrDefaultAsync(todo => todo.Id == id);
        if (tareaExistente is null)
        {
            return null;
        }

        ValidarTodo(dto);
        await ValidarCategoriaAsync(dto.CategoriaId);

        tareaExistente.Title = dto.Titulo;
        tareaExistente.IsCompleted = dto.Completada;
        tareaExistente.EsRepetitiva = dto.EsRepetitiva;
        tareaExistente.Recurrencia = dto.Recurrencia;
        tareaExistente.ProximaFecha = dto.EsRepetitiva && dto.Recurrencia.HasValue ? CalcularProximaFecha(dto.Recurrencia.Value, DateTime.UtcNow) : null;
        tareaExistente.CategoriaId = dto.CategoriaId;

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

    public async Task<TareaDto?> CompletarAsync(int id)
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

    private async Task ValidarCategoriaAsync(int? categoriaId)
    {
        if (categoriaId is null)
        {
            return;
        }

        var categoria = await _categoriaLogica.ObtenerPorIdAsync(categoriaId.Value);
        if (categoria is null)
        {
            throw new ArgumentException("La categoría especificada no existe.");
        }
    }

    private static void ValidarTodo(GuardarTareaDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Titulo))
        {
            throw new ArgumentException("El título es obligatorio.");
        }

        if (dto.EsRepetitiva && !dto.Recurrencia.HasValue)
        {
            throw new ArgumentException("La recurrencia es obligatoria para tareas repetitivas.");
        }
    }

    private static TareaDto MapearADto(TodoItem tarea) => new()
    {
        Id = tarea.Id,
        Titulo = tarea.Title,
        Completada = tarea.IsCompleted,
        CreatedAt = tarea.CreatedAt,
        EsRepetitiva = tarea.EsRepetitiva,
        Recurrencia = tarea.Recurrencia,
        ProximaFecha = tarea.ProximaFecha,
        PlantillaId = tarea.PlantillaId,
        CategoriaId = tarea.CategoriaId,
        Categoria = tarea.Categoria is null ? null : new CategoriaDto
        {
            Id = tarea.Categoria.Id,
            Nombre = tarea.Categoria.Nombre,
            Color = tarea.Categoria.Color
        },
        PersonaId = tarea.PersonaId
    };

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

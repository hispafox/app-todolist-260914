using AppTodoList.Api.Data;
using AppTodoList.Models;
using Microsoft.EntityFrameworkCore;

namespace AppTodoList.Api.Services;

public class PlantillaService : IPlantillaService
{
    private readonly AppDbContext _context;

    public PlantillaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PlantillaTarea>> ObtenerTodasAsync()
    {
        return await _context.Plantillas
            .AsNoTracking()
            .OrderBy(plantilla => plantilla.Id)
            .ToListAsync();
    }

    public async Task<PlantillaTarea?> ObtenerPorIdAsync(int id)
    {
        return await _context.Plantillas
            .AsNoTracking()
            .FirstOrDefaultAsync(plantilla => plantilla.Id == id);
    }

    public async Task<PlantillaTarea> CrearAsync(PlantillaTarea plantilla)
    {
        ValidarPlantilla(plantilla);

        _context.Plantillas.Add(plantilla);
        await _context.SaveChangesAsync();
        return plantilla;
    }

    public async Task<PlantillaTarea?> ActualizarAsync(int id, PlantillaTarea plantilla)
    {
        var plantillaExistente = await _context.Plantillas.FirstOrDefaultAsync(item => item.Id == id);
        if (plantillaExistente is null)
        {
            return null;
        }

        ValidarPlantilla(plantilla);

        plantillaExistente.Titulo = plantilla.Titulo;
        plantillaExistente.EsRepetitiva = plantilla.EsRepetitiva;
        plantillaExistente.Recurrencia = plantilla.Recurrencia;

        await _context.SaveChangesAsync();
        return plantillaExistente;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var plantilla = await _context.Plantillas.FirstOrDefaultAsync(item => item.Id == id);
        if (plantilla is null)
        {
            return false;
        }

        _context.Plantillas.Remove(plantilla);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<TodoItem?> InstanciarAsync(int id)
    {
        var plantilla = await _context.Plantillas.FirstOrDefaultAsync(item => item.Id == id);
        if (plantilla is null)
        {
            return null;
        }

        var tarea = new TodoItem
        {
            Title = plantilla.Titulo,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow,
            EsRepetitiva = plantilla.EsRepetitiva,
            Recurrencia = plantilla.Recurrencia,
            ProximaFecha = plantilla.EsRepetitiva && plantilla.Recurrencia.HasValue
                ? CalcularProximaFecha(plantilla.Recurrencia.Value, DateTime.UtcNow)
                : null,
            PlantillaId = plantilla.Id,
            CategoriaId = null,
            PersonaId = null
        };

        _context.TodoItems.Add(tarea);
        await _context.SaveChangesAsync();
        return tarea;
    }

    private static void ValidarPlantilla(PlantillaTarea plantilla)
    {
        if (string.IsNullOrWhiteSpace(plantilla.Titulo))
        {
            throw new ArgumentException("El título de la plantilla es obligatorio.");
        }

        if (plantilla.EsRepetitiva && !plantilla.Recurrencia.HasValue)
        {
            throw new ArgumentException("La recurrencia es obligatoria para plantillas repetitivas.");
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

using AppTodoList.Api.Data;
using AppTodoList.Api.Dtos;
using AppTodoList.Api.LogicaNegocio;
using AppTodoList.Api.Services;
using AppTodoList.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AppTodoList.Api.Tests.Servicios;

public class TodoServiceTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly AppDbContext _context;
    private readonly TodoService _service;

    public TodoServiceTests()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new AppDbContext(options);
        _context.Database.EnsureCreated();

        var categoriaLogica = new CategoriaLogica(_context);
        _service = new TodoService(_context, categoriaLogica);
    }

    [Fact]
    public async Task CrearAsync_ConCategoriaExistente_DeberiaAsignarla()
    {
        var dto = new GuardarTareaDto
        {
            Titulo = "Preparar reunión",
            Completada = false,
            EsRepetitiva = false,
            CategoriaId = 2
        };

        var creada = await _service.CrearAsync(dto);

        Assert.Equal(2, creada.CategoriaId);
        Assert.NotNull(creada.Categoria);
        Assert.Equal("Trabajo", creada.Categoria!.Nombre);
    }

    [Fact]
    public async Task CrearAsync_ConCategoriaInexistente_DeberiaLanzarArgumentException()
    {
        var dto = new GuardarTareaDto
        {
            Titulo = "Tarea inválida",
            CategoriaId = 999
        };

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CrearAsync(dto));
    }

    [Fact]
    public async Task ActualizarAsync_ConCategoriaNula_DeberiaQuitarLaAsignacion()
    {
        var dto = new GuardarTareaDto
        {
            Titulo = "Tarea sin categoría",
            Completada = false,
            EsRepetitiva = false,
            CategoriaId = null
        };

        var actualizada = await _service.ActualizarAsync(1, dto);

        Assert.NotNull(actualizada);
        Assert.Null(actualizada!.CategoriaId);
    }

    [Fact]
    public async Task CompletarAsync_TareaRepetitivaConCategoria_DeberiaConservarCategoriaEnLaSiguienteOcurrencia()
    {
        var tarea = new TodoItem
        {
            Id = 101,
            Title = "Enviar informe",
            CreatedAt = DateTime.UtcNow,
            EsRepetitiva = true,
            Recurrencia = TipoRecurrencia.Semanal,
            ProximaFecha = DateTime.UtcNow.AddDays(7),
            CategoriaId = 2
        };

        _context.TodoItems.Add(tarea);
        await _context.SaveChangesAsync();

        var completada = await _service.CompletarAsync(101);

        Assert.NotNull(completada);
        Assert.Equal(2, completada!.CategoriaId);
        Assert.Equal(2, _context.TodoItems.Count(todo => todo.Title == "Enviar informe" && !todo.IsCompleted));
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}

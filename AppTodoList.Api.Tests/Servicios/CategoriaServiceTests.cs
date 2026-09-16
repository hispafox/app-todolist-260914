using AppTodoList.Api.Data;
using AppTodoList.Api.LogicaNegocio;
using AppTodoList.Api.Services;
using AppTodoList.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AppTodoList.Api.Tests.Servicios;

public class CategoriaServiceTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly AppDbContext _context;
    private readonly CategoriaService _service;

    public CategoriaServiceTests()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new AppDbContext(options);
        _context.Database.EnsureCreated();
        _service = new CategoriaService(new CategoriaLogica(_context));
    }

    [Fact]
    public async Task ObtenerCategoriasAsync_DeberiaDevolverCategoriasOrdenadasPorNombre()
    {
        var categorias = await _service.ObtenerTodosAsync();

        Assert.Equal("Hogar", categorias.First().Nombre);
        Assert.Equal("Trabajo", categorias.Last().Nombre);
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}

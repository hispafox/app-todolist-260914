using AppTodoList.Api.Controllers;
using AppTodoList.Api.Data;
using AppTodoList.Api.Dtos;
using AppTodoList.Api.LogicaNegocio;
using AppTodoList.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AppTodoList.Api.Tests.Controllers;

public class CategoriasControllerTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly AppDbContext _context;
    private readonly CategoriasController _controller;

    public CategoriasControllerTests()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new AppDbContext(options);
        _context.Database.EnsureCreated();

        var categoriaService = new CategoriaService(new CategoriaLogica(_context));
        _controller = new CategoriasController(categoriaService);
    }

    [Fact]
    public async Task GetAll_DeberiaDevolverCategoriaDto()
    {
        var resultado = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
        var categorias = Assert.IsAssignableFrom<IEnumerable<CategoriaDto>>(okResult.Value);
        Assert.NotEmpty(categorias);
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}

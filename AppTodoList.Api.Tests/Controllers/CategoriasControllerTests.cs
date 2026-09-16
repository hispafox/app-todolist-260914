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

    [Fact]
    public async Task Create_ConDatosValidos_DeberiaDevolver201ConLaCategoriaCreada()
    {
        var dto = new GuardarCategoriaDto { Nombre = "Urgente", Color = "#ff0000" };

        var resultado = await _controller.Create(dto);

        var creadoResult = Assert.IsType<CreatedResult>(resultado.Result);
        var categoria = Assert.IsType<CategoriaDto>(creadoResult.Value);
        Assert.True(categoria.Id > 0);
        Assert.Equal("Urgente", categoria.Nombre);
    }

    [Fact]
    public async Task Create_ConNombreVacio_DeberiaDevolver400()
    {
        var dto = new GuardarCategoriaDto { Nombre = "   ", Color = "#ff0000" };

        var resultado = await _controller.Create(dto);

        Assert.IsType<BadRequestObjectResult>(resultado.Result);
    }

    [Fact]
    public async Task Create_ConNombreDemasiadoLargo_DeberiaDevolver400()
    {
        var dto = new GuardarCategoriaDto { Nombre = new string('a', 101), Color = "#ff0000" };

        var resultado = await _controller.Create(dto);

        Assert.IsType<BadRequestObjectResult>(resultado.Result);
    }

    [Fact]
    public async Task Create_DeberiaPersistirLaCategoriaEnElContexto()
    {
        var dto = new GuardarCategoriaDto { Nombre = "Personal", Color = "#00ff00" };

        await _controller.Create(dto);
        var resultado = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
        var categorias = Assert.IsAssignableFrom<IEnumerable<CategoriaDto>>(okResult.Value);
        Assert.Contains(categorias, c => c.Nombre == "Personal");
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}

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

    [Fact]
    public async Task GetById_ConIdExistente_DeberiaDevolver200ConLaCategoria()
    {
        var creada = await _controller.Create(new GuardarCategoriaDto { Nombre = "Trabajo", Color = "#0000ff" });
        var creadaDto = Assert.IsType<CategoriaDto>(Assert.IsType<CreatedResult>(creada.Result).Value);

        var resultado = await _controller.GetById(creadaDto.Id);

        var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
        var categoria = Assert.IsType<CategoriaDto>(okResult.Value);
        Assert.Equal("Trabajo", categoria.Nombre);
    }

    [Fact]
    public async Task GetById_ConIdInexistente_DeberiaDevolver404()
    {
        var resultado = await _controller.GetById(9999);

        Assert.IsType<NotFoundResult>(resultado.Result);
    }

    [Fact]
    public async Task Update_ConDatosValidos_DeberiaDevolver200ConLaCategoriaActualizada()
    {
        var creada = await _controller.Create(new GuardarCategoriaDto { Nombre = "Original", Color = "#111111" });
        var creadaDto = Assert.IsType<CategoriaDto>(Assert.IsType<CreatedResult>(creada.Result).Value);

        var resultado = await _controller.Update(creadaDto.Id, new GuardarCategoriaDto { Nombre = "Actualizada", Color = "#222222" });

        var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
        var categoria = Assert.IsType<CategoriaDto>(okResult.Value);
        Assert.Equal("Actualizada", categoria.Nombre);
        Assert.Equal("#222222", categoria.Color);
    }

    [Fact]
    public async Task Update_ConIdInexistente_DeberiaDevolver404()
    {
        var resultado = await _controller.Update(9999, new GuardarCategoriaDto { Nombre = "Nueva", Color = "#333333" });

        Assert.IsType<NotFoundResult>(resultado.Result);
    }

    [Fact]
    public async Task Update_ConNombreVacio_DeberiaDevolver400()
    {
        var creada = await _controller.Create(new GuardarCategoriaDto { Nombre = "Original", Color = "#111111" });
        var creadaDto = Assert.IsType<CategoriaDto>(Assert.IsType<CreatedResult>(creada.Result).Value);

        var resultado = await _controller.Update(creadaDto.Id, new GuardarCategoriaDto { Nombre = "   ", Color = "#111111" });

        Assert.IsType<BadRequestObjectResult>(resultado.Result);
    }

    [Fact]
    public async Task Update_ConNombreDemasiadoLargo_DeberiaDevolver400()
    {
        var creada = await _controller.Create(new GuardarCategoriaDto { Nombre = "Original", Color = "#111111" });
        var creadaDto = Assert.IsType<CategoriaDto>(Assert.IsType<CreatedResult>(creada.Result).Value);

        var resultado = await _controller.Update(creadaDto.Id, new GuardarCategoriaDto { Nombre = new string('a', 101), Color = "#111111" });

        Assert.IsType<BadRequestObjectResult>(resultado.Result);
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}

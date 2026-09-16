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

public class TareasControllerTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly AppDbContext _context;
    private readonly TareasController _controller;

    public TareasControllerTests()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new AppDbContext(options);
        _context.Database.EnsureCreated();

        var todoService = new TodoService(_context, new CategoriaLogica(_context));
        _controller = new TareasController(todoService);
    }

    [Fact]
    public async Task Create_ConCategoriaInexistente_DeberiaDevolverBadRequest()
    {
        var dto = new GuardarTareaDto
        {
            Titulo = "Tarea inválida",
            CategoriaId = 999
        };

        var resultado = await _controller.Create(dto);

        Assert.IsType<BadRequestObjectResult>(resultado.Result);
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}

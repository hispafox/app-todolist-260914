using AppTodoList.Api.Data;
using AppTodoList.Models;
using Microsoft.EntityFrameworkCore;

namespace AppTodoList.Api.LogicaNegocio;

public class CategoriaLogica : ICategoriaLogica
{
    private readonly AppDbContext _contexto;

    public CategoriaLogica(AppDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<IEnumerable<Categoria>> ObtenerTodosAsync()
        => await _contexto.Categorias
            .AsNoTracking()
            .OrderBy(categoria => categoria.Nombre)
            .ToListAsync();

    public async Task<Categoria?> ObtenerPorIdAsync(int id)
        => await _contexto.Categorias
            .AsNoTracking()
            .FirstOrDefaultAsync(categoria => categoria.Id == id);

    public async Task<Categoria?> CrearAsync(Categoria categoria)
    {
        ValidarCategoria(categoria);

        _contexto.Categorias.Add(categoria);
        await _contexto.SaveChangesAsync();
        return categoria;
    }

    public async Task<Categoria?> ActualizarAsync(int id, Categoria categoria)
    {
        var existente = await _contexto.Categorias.FirstOrDefaultAsync(c => c.Id == id);
        if (existente is null)
        {
            return null;
        }

        ValidarCategoria(categoria);

        existente.Nombre = categoria.Nombre;
        existente.Color = categoria.Color;

        await _contexto.SaveChangesAsync();
        return existente;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var existente = await _contexto.Categorias.FirstOrDefaultAsync(c => c.Id == id);
        if (existente is null)
        {
            return false;
        }

        _contexto.Categorias.Remove(existente);
        await _contexto.SaveChangesAsync();
        return true;
    }

    private static void ValidarCategoria(Categoria categoria)
    {
        if (string.IsNullOrWhiteSpace(categoria.Nombre))
        {
            throw new ArgumentException("El nombre de la categoría es obligatorio.");
        }

        if (categoria.Nombre.Length > 100)
        {
            throw new ArgumentException("El nombre de la categoría no puede superar 100 caracteres.");
        }
    }
}

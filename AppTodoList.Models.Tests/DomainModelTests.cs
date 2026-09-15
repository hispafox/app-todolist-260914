using AppTodoList.Models;
using Xunit;

namespace AppTodoList.Models.Tests;

public class DomainModelTests
{
    [Fact]
    public void TodoItem_DeberiaIncluirLosCamposDefinidosEnElAnalisis()
    {
        var item = new TodoItem
        {
            Id = 1,
            Title = "Comprar pan",
            IsCompleted = false,
            CreatedAt = new DateTime(2026, 9, 15, 8, 30, 0),
            EsRepetitiva = true,
            Recurrencia = TipoRecurrencia.Diaria,
            ProximaFecha = new DateTime(2026, 9, 16, 8, 30, 0),
            PlantillaId = 7
        };

        Assert.Equal(1, item.Id);
        Assert.Equal("Comprar pan", item.Title);
        Assert.False(item.IsCompleted);
        Assert.Equal(new DateTime(2026, 9, 15, 8, 30, 0), item.CreatedAt);
        Assert.True(item.EsRepetitiva);
        Assert.Equal(TipoRecurrencia.Diaria, item.Recurrencia);
        Assert.Equal(new DateTime(2026, 9, 16, 8, 30, 0), item.ProximaFecha);
        Assert.Equal(7, item.PlantillaId);
        Assert.Null(item.Plantilla);
    }

    [Fact]
    public void PlantillaTarea_DeberiaDefinirLosCamposDeLaPlantilla()
    {
        var plantilla = new PlantillaTarea
        {
            Id = 10,
            Titulo = "Preparar comida",
            EsRepetitiva = true,
            Recurrencia = TipoRecurrencia.Semanal
        };

        Assert.Equal(10, plantilla.Id);
        Assert.Equal("Preparar comida", plantilla.Titulo);
        Assert.True(plantilla.EsRepetitiva);
        Assert.Equal(TipoRecurrencia.Semanal, plantilla.Recurrencia);
    }

    [Fact]
    public void TodoItem_DeberiaPermitirAsociarUnaCategoria()
    {
        var categoria = new Categoria
        {
            Id = 5,
            Nombre = "Hogar",
            Color = "#4CAF50"
        };

        var item = new TodoItem
        {
            Id = 2,
            Title = "Ordenar la cocina",
            CategoriaId = categoria.Id,
            Categoria = categoria
        };

        Assert.Equal(5, item.CategoriaId);
        Assert.Equal("Hogar", item.Categoria!.Nombre);
        Assert.Equal("#4CAF50", item.Categoria.Color);
    }

    [Fact]
    public void TodoItem_DeberiaPermitirAsignarUnaPersona()
    {
        var persona = new Persona
        {
            Id = 9,
            Nombre = "Ana López"
        };

        var item = new TodoItem
        {
            Id = 3,
            Title = "Revisar presupuesto",
            PersonaId = persona.Id,
            Persona = persona
        };

        Assert.Equal(9, item.PersonaId);
        Assert.Equal("Ana López", item.Persona!.Nombre);
    }

    [Fact]
    public void Persona_DeberiaDefinirLosCamposDeLaPersona()
    {
        var persona = new Persona
        {
            Id = 12,
            Nombre = "Lucía Gómez"
        };

        Assert.Equal(12, persona.Id);
        Assert.Equal("Lucía Gómez", persona.Nombre);
    }

    [Fact]
    public void TipoRecurrencia_DeberiaExponerLosValoresDelDominio()
    {
        var valores = Enum.GetValues<TipoRecurrencia>();

        Assert.Equal(3, valores.Length);
        Assert.Contains(TipoRecurrencia.Diaria, valores);
        Assert.Contains(TipoRecurrencia.Semanal, valores);
        Assert.Contains(TipoRecurrencia.Mensual, valores);
    }
}

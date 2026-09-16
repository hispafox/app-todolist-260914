using AppTodoList.Models;

namespace AppTodoList.Api.Dtos;

public class TareaDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public bool Completada { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool EsRepetitiva { get; set; }
    public TipoRecurrencia? Recurrencia { get; set; }
    public DateTime? ProximaFecha { get; set; }
    public int? PlantillaId { get; set; }
    public int? CategoriaId { get; set; }
    public CategoriaDto? Categoria { get; set; }
    public int? PersonaId { get; set; }
}

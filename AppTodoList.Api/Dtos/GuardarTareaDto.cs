using System.ComponentModel.DataAnnotations;

namespace AppTodoList.Api.Dtos;

public class GuardarTareaDto
{
    [Required(ErrorMessage = "El título es obligatorio.")]
    [MaxLength(200, ErrorMessage = "El título no puede superar 200 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    public bool Completada { get; set; }
    public bool EsRepetitiva { get; set; }
    public AppTodoList.Models.TipoRecurrencia? Recurrencia { get; set; }
    public int? CategoriaId { get; set; }
}

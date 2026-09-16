using System.ComponentModel.DataAnnotations;

namespace AppTodoList.Api.Dtos;

public class GuardarCategoriaDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El nombre no puede superar 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(30, ErrorMessage = "El color no puede superar 30 caracteres.")]
    public string Color { get; set; } = string.Empty;
}

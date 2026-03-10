using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO;

public class TipoEventoDTO
{
    [Required(ErrorMessage = "O titulo do tipo evento é obrigatório!")]
    public string? Titulo { get; set; }
}

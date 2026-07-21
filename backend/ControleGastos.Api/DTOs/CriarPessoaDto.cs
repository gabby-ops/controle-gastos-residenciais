using System.ComponentModel.DataAnnotations;

namespace ControleGastos.Api.DTOs;

public class CriarPessoaDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "A idade é obrigatória.")]
    [Range(0, int.MaxValue, ErrorMessage = "A idade não pode ser negativa.")]
    public int Idade { get; set; }
}
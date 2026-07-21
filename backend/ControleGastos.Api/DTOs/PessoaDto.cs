namespace ControleGastos.Api.DTOs;

// DTO de saída: expõe só o que o frontend precisa, sem a lista de transações
// (evita respostas gigantes e problemas de serialização circular).
public class PessoaDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Idade { get; set; }
}
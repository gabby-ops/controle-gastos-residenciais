namespace ControleGastos.Api.DTOs;

public class TotalPessoaDto
{
    public string Nome { get; set; } = string.Empty;
    public decimal TotalReceitas { get; set; }
    public decimal TotalDespesas { get; set; }
    public decimal Saldo => TotalReceitas - TotalDespesas;
}

public class TotaisResponseDto
{
    public List<TotalPessoaDto> Pessoas { get; set; } = new();
    public decimal TotalGeralReceitas { get; set; }
    public decimal TotalGeralDespesas { get; set; }
    public decimal SaldoLiquidoGeral => TotalGeralReceitas - TotalGeralDespesas;
}
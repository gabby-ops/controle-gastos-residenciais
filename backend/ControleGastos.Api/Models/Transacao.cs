namespace ControleGastos.Api.Models;

/// <summary>
/// Tipos de transação permitidos. Usar enum evita valores inválidos
/// e facilita a validação da regra de negócio do menor de idade.
/// </summary>
public enum TipoTransacao
{
    Receita = 0,
    Despesa = 1
}

public class Transacao
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public TipoTransacao Tipo { get; set; }

    public int PessoaId { get; set; }
    public Pessoa? Pessoa { get; set; }
}
namespace ControleGastos.Api.Models;

/// <summary>
/// Representa uma pessoa cadastrada no sistema.
/// Uma pessoa pode ter várias transações associadas (relacionamento 1:N).
/// </summary>
public class Pessoa
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Idade { get; set; }

    // Propriedade de navegação para o relacionamento 1:N.
    // O EF Core usa isso para configurar o Cascade Delete.
    public ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();
}
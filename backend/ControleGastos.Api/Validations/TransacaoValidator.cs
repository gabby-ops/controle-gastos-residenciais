using ControleGastos.Api.Models;

namespace ControleGastos.Api.Validations;

/// <summary>
/// Centraliza as regras de negócio de validação de transações,
/// mantendo o Service enxuto e a regra fácil de testar isoladamente.
/// </summary>
public static class TransacaoValidator
{
    public const int IdadeMinimaParaReceita = 18;

    /// <summary>
    /// Regra de negócio: pessoas menores de 18 anos só podem
    /// cadastrar Despesas. Receitas são bloqueadas para menores.
    /// </summary>
    public static (bool valido, string? erro) ValidarRegraMenorIdade(Pessoa pessoa, TipoTransacao tipo)
    {
        if (pessoa.Idade < IdadeMinimaParaReceita && tipo == TipoTransacao.Receita)
        {
            return (false, $"Pessoas menores de {IdadeMinimaParaReceita} anos não podem cadastrar receitas, apenas despesas.");
        }

        return (true, null);
    }
}
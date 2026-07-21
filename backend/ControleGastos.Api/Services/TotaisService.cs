using ControleGastos.Api.DTOs;
using ControleGastos.Api.Models;
using ControleGastos.Api.Repositories;

namespace ControleGastos.Api.Services;

/// <summary>
/// Calcula o resumo financeiro por pessoa (receitas, despesas, saldo)
/// e os totais gerais somando todas as pessoas.
/// </summary>
public class TotaisService : ITotaisService
{
    private readonly IPessoaRepository _pessoaRepository;
    private readonly ITransacaoRepository _transacaoRepository;

    public TotaisService(IPessoaRepository pessoaRepository, ITransacaoRepository transacaoRepository)
    {
        _pessoaRepository = pessoaRepository;
        _transacaoRepository = transacaoRepository;
    }

    /// <summary>
    /// Para cada pessoa, soma as transações do tipo Receita e do tipo
    /// Despesa separadamente. O saldo (Receitas - Despesas) é calculado
    /// como propriedade computada no próprio DTO. Ao final, os totais
    /// gerais são a soma de todas as pessoas.
    /// </summary>
    public async Task<TotaisResponseDto> ObterTotaisAsync()
    {
        var pessoas = await _pessoaRepository.ObterTodosAsync();
        var transacoes = await _transacaoRepository.ObterTodasAsync();

        var resultado = new TotaisResponseDto();

        foreach (var pessoa in pessoas)
        {
            var transacoesDaPessoa = transacoes.Where(t => t.PessoaId == pessoa.Id).ToList();

            var receitas = transacoesDaPessoa
                .Where(t => t.Tipo == TipoTransacao.Receita)
                .Sum(t => t.Valor);

            var despesas = transacoesDaPessoa
                .Where(t => t.Tipo == TipoTransacao.Despesa)
                .Sum(t => t.Valor);

            resultado.Pessoas.Add(new TotalPessoaDto
            {
                Nome = pessoa.Nome,
                TotalReceitas = receitas,
                TotalDespesas = despesas
            });

            resultado.TotalGeralReceitas += receitas;
            resultado.TotalGeralDespesas += despesas;
        }

        return resultado;
    }
}
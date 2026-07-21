using ControleGastos.Api.DTOs;
using ControleGastos.Api.Models;
using ControleGastos.Api.Repositories;
using ControleGastos.Api.Validations;

namespace ControleGastos.Api.Services;

/// <summary>
/// Contém a lógica de negócio do cadastro de transações: validação
/// da pessoa, aplicação da regra do menor de idade e mapeamento
/// entre entidade e DTO.
/// </summary>
public class TransacaoService : ITransacaoService
{
    private readonly ITransacaoRepository _transacaoRepository;
    private readonly IPessoaRepository _pessoaRepository;

    public TransacaoService(ITransacaoRepository transacaoRepository, IPessoaRepository pessoaRepository)
    {
        _transacaoRepository = transacaoRepository;
        _pessoaRepository = pessoaRepository;
    }

    public async Task<List<TransacaoDto>> ObterTodasAsync()
    {
        var transacoes = await _transacaoRepository.ObterTodasAsync();
        return transacoes.Select(MapToDto).ToList();
    }

    /// <summary>
    /// Cria uma transação após validar:
    /// 1) que a pessoa informada existe;
    /// 2) que, se a pessoa for menor de 18 anos, o tipo não seja Receita.
    /// Retorna uma tupla indicando sucesso/erro em vez de lançar exceção,
    /// pois isso representa uma falha de validação de negócio esperada,
    /// não um erro inesperado do sistema.
    /// </summary>
    public async Task<(bool sucesso, string? erro, TransacaoDto? dto)> CriarAsync(CriarTransacaoDto dto)
    {
        var pessoa = await _pessoaRepository.ObterPorIdAsync(dto.PessoaId);
        if (pessoa is null)
        {
            return (false, "Pessoa não encontrada.", null);
        }

        var (valido, erroRegra) = TransacaoValidator.ValidarRegraMenorIdade(pessoa, dto.Tipo);
        if (!valido)
        {
            return (false, erroRegra, null);
        }

        var transacao = new Transacao
        {
            Descricao = dto.Descricao,
            Valor = dto.Valor,
            Tipo = dto.Tipo,
            PessoaId = dto.PessoaId
        };

        var criada = await _transacaoRepository.CriarAsync(transacao);
        criada.Pessoa = pessoa; // evita um SELECT extra, já temos a pessoa em memória

        return (true, null, MapToDto(criada));
    }

    /// <summary>Converte a entidade Transacao (com Pessoa carregada) em DTO de saída.</summary>
    private static TransacaoDto MapToDto(Transacao t) => new()
    {
        Id = t.Id,
        Descricao = t.Descricao,
        Valor = t.Valor,
        Tipo = t.Tipo,
        PessoaId = t.PessoaId,
        PessoaNome = t.Pessoa?.Nome ?? string.Empty
    };
}
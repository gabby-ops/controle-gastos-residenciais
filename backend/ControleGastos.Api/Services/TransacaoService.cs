using ControleGastos.Api.DTOs;
using ControleGastos.Api.Models;
using ControleGastos.Api.Repositories;
using ControleGastos.Api.Validations;

namespace ControleGastos.Api.Services;

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

    public async Task<(bool sucesso, string? erro, TransacaoDto? dto)> CriarAsync(CriarTransacaoDto dto)
    {
        var pessoa = await _pessoaRepository.ObterPorIdAsync(dto.PessoaId);
        if (pessoa is null)
        {
            return (false, "Pessoa não encontrada.", null);
        }

        // Regra de negócio central do sistema: menores de idade
        // não podem cadastrar receitas.
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
        criada.Pessoa = pessoa; // já temos os dados em memória, evita novo select

        return (true, null, MapToDto(criada));
    }

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
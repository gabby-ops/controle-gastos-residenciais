using ControleGastos.Api.DTOs;
using ControleGastos.Api.Models;
using ControleGastos.Api.Repositories;

namespace ControleGastos.Api.Services;

/// <summary>
/// Contém a lógica de negócio do cadastro de pessoas, incluindo a
/// validação de nome duplicado antes de persistir.
/// </summary>
public class PessoaService : IPessoaService
{
    private readonly IPessoaRepository _repository;

    public PessoaService(IPessoaRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<PessoaDto>> ObterTodosAsync()
    {
        var pessoas = await _repository.ObterTodosAsync();
        return pessoas.Select(MapToDto).ToList();
    }

    /// <summary>
    /// Cria uma nova pessoa após validar que não existe outra pessoa
    /// já cadastrada com o mesmo nome (evita duplicidade acidental).
    /// Retorna uma tupla indicando sucesso/erro em vez de lançar exceção,
    /// pois isso representa uma falha de validação de negócio esperada.
    /// </summary>
    public async Task<(bool sucesso, string? erro, PessoaDto? dto)> CriarAsync(CriarPessoaDto dto)
    {
        var nomeDuplicado = await _repository.ExisteComNomeAsync(dto.Nome);
        if (nomeDuplicado)
        {
            return (false, "Já existe uma pessoa cadastrada com esse nome.", null);
        }

        var pessoa = new Pessoa
        {
            Nome = dto.Nome,
            Idade = dto.Idade
        };

        var criada = await _repository.CriarAsync(pessoa);
        return (true, null, MapToDto(criada));
    }

    public async Task<bool> ExcluirAsync(int id) => await _repository.ExcluirAsync(id);

    /// <summary>Converte a entidade Pessoa em DTO de saída.</summary>
    private static PessoaDto MapToDto(Pessoa p) => new()
    {
        Id = p.Id,
        Nome = p.Nome,
        Idade = p.Idade
    };
}
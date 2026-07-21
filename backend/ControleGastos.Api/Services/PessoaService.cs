using ControleGastos.Api.DTOs;
using ControleGastos.Api.Models;
using ControleGastos.Api.Repositories;

namespace ControleGastos.Api.Services;

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

    public async Task<PessoaDto> CriarAsync(CriarPessoaDto dto)
    {
        var pessoa = new Pessoa
        {
            Nome = dto.Nome,
            Idade = dto.Idade
        };

        var criada = await _repository.CriarAsync(pessoa);
        return MapToDto(criada);
    }

    public async Task<bool> ExcluirAsync(int id) => await _repository.ExcluirAsync(id);

    private static PessoaDto MapToDto(Pessoa p) => new()
    {
        Id = p.Id,
        Nome = p.Nome,
        Idade = p.Idade
    };
}
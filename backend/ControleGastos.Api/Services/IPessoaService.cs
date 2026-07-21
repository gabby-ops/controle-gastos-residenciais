using ControleGastos.Api.DTOs;

namespace ControleGastos.Api.Services;

public interface IPessoaService
{
    Task<List<PessoaDto>> ObterTodosAsync();

    /// <summary>
    /// Retorna (sucesso, erro, dto). Falha quando já existe uma pessoa
    /// cadastrada com o mesmo nome.
    /// </summary>
    Task<(bool sucesso, string? erro, PessoaDto? dto)> CriarAsync(CriarPessoaDto dto);

    Task<bool> ExcluirAsync(int id);
}
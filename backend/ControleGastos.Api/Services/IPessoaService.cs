using ControleGastos.Api.DTOs;

namespace ControleGastos.Api.Services;

public interface IPessoaService
{
    Task<List<PessoaDto>> ObterTodosAsync();
    Task<PessoaDto> CriarAsync(CriarPessoaDto dto);
    Task<bool> ExcluirAsync(int id);
}
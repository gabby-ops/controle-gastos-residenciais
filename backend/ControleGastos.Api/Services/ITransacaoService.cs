using ControleGastos.Api.DTOs;

namespace ControleGastos.Api.Services;

public interface ITransacaoService
{
    Task<List<TransacaoDto>> ObterTodasAsync();

    /// <summary>
    /// Retorna (sucesso, erro, dto). Em caso de falha de validação,
    /// sucesso é false e erro contém a mensagem apropriada.
    /// </summary>
    Task<(bool sucesso, string? erro, TransacaoDto? dto)> CriarAsync(CriarTransacaoDto dto);
}
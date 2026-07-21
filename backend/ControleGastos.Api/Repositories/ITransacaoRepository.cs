using ControleGastos.Api.Models;

namespace ControleGastos.Api.Repositories;

public interface ITransacaoRepository
{
    Task<List<Transacao>> ObterTodasAsync();
    Task<Transacao> CriarAsync(Transacao transacao);
}
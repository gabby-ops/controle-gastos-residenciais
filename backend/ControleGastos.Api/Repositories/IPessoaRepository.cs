using ControleGastos.Api.Models;

namespace ControleGastos.Api.Repositories;

public interface IPessoaRepository
{
    Task<List<Pessoa>> ObterTodosAsync();
    Task<Pessoa?> ObterPorIdAsync(int id);
    Task<Pessoa> CriarAsync(Pessoa pessoa);
    Task<bool> ExcluirAsync(int id);
    Task<bool> ExisteAsync(int id);

    /// <summary>
    /// Verifica se já existe uma pessoa cadastrada com o mesmo nome
    /// (comparação sem diferenciar maiúsculas/minúsculas).
    /// </summary>
    Task<bool> ExisteComNomeAsync(string nome);
}
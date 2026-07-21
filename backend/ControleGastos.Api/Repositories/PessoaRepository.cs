using ControleGastos.Api.Data;
using ControleGastos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleGastos.Api.Repositories;

/// <summary>
/// Implementa o acesso a dados da entidade Pessoa usando Entity Framework Core.
/// </summary>
public class PessoaRepository : IPessoaRepository
{
    private readonly AppDbContext _context;

    public PessoaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Pessoa>> ObterTodosAsync() =>
        await _context.Pessoas.AsNoTracking().ToListAsync();

    public async Task<Pessoa?> ObterPorIdAsync(int id) =>
        await _context.Pessoas.FindAsync(id);

    public async Task<Pessoa> CriarAsync(Pessoa pessoa)
    {
        _context.Pessoas.Add(pessoa);
        await _context.SaveChangesAsync();
        return pessoa;
    }

    public async Task<bool> ExcluirAsync(int id)
    {
        var pessoa = await _context.Pessoas.FindAsync(id);
        if (pessoa is null) return false;

        // O Cascade Delete configurado no DbContext remove as
        // transações vinculadas automaticamente no banco.
        _context.Pessoas.Remove(pessoa);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExisteAsync(int id) =>
        await _context.Pessoas.AnyAsync(p => p.Id == id);

    /// <summary>
    /// Compara nomes ignorando maiúsculas/minúsculas usando ToLower(),
    /// já que o SQLite não garante comparação case-insensitive por padrão
    /// com o operador de igualdade em todas as configurações.
    /// </summary>
    public async Task<bool> ExisteComNomeAsync(string nome) =>
        await _context.Pessoas.AnyAsync(p => p.Nome.ToLower() == nome.ToLower());
}
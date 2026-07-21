using ControleGastos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleGastos.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Pessoa> Pessoas => Set<Pessoa>();
    public DbSet<Transacao> Transacoes => Set<Transacao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Pessoa>(entity =>
        {
            entity.Property(p => p.Nome).IsRequired();
        });

        modelBuilder.Entity<Transacao>(entity =>
        {
            entity.Property(t => t.Descricao).IsRequired();
            entity.Property(t => t.Valor).HasColumnType("decimal(18,2)");

            // Relacionamento 1:N com Cascade Delete:
            // ao excluir uma Pessoa, todas as Transacoes vinculadas são apagadas.
            entity.HasOne(t => t.Pessoa)
                  .WithMany(p => p.Transacoes)
                  .HasForeignKey(t => t.PessoaId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
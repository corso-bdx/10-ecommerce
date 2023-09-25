using Microsoft.EntityFrameworkCore;

namespace Bdx.ECommerce;

public class ECommerceDbContext : DbContext
{
    public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options) { }

    public DbSet<Utente> ListaUtente { get; set; } = null!;

    public DbSet<Prodotto> ListaProdotto { get; set; } = null!;

    public DbSet<Acquisto> ListaAcquisto { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Utente>().HasKey(t => new { t.Id });
        modelBuilder.Entity<Utente>().HasMany(p => p.Acquisti).WithOne(p => p.Utente);

        modelBuilder.Entity<Prodotto>().HasKey(t => new { t.Id });
        modelBuilder.Entity<Prodotto>().HasMany(p => p.Acquisti).WithOne(p => p.Prodotto);

        modelBuilder.Entity<Acquisto>().HasKey(t => new { t.Id });
        modelBuilder.Entity<Acquisto>().HasOne(p => p.Utente).WithMany(p => p.Acquisti).HasForeignKey(p => p.IdUtente);
        modelBuilder.Entity<Acquisto>().HasOne(p => p.Prodotto).WithMany(p => p.Acquisti).HasForeignKey(p => p.IdProdotto);

        base.OnModelCreating(modelBuilder);
    }
}

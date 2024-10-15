using gol_backend_api.Models;
using Microsoft.EntityFrameworkCore;

namespace gol_backend_api.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Vehicle> Veiculos { get; set; }
    public DbSet<Car> Carros { get; set; }
    public DbSet<Truck> Caminhoes { get; set; }
    public DbSet<Revision> Revisoes { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>()
            .HasBaseType<Vehicle>();
        
        modelBuilder.Entity<Truck>()
            .HasBaseType<Vehicle>();
        
        modelBuilder.Entity<Revision>()
            .HasOne(r => r.Veiculo)
            .WithMany(v => v.Revisoes)
            .HasForeignKey(r => r.VeiculoId);
    }

}
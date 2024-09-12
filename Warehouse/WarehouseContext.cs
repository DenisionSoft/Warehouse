using Microsoft.EntityFrameworkCore;
using Warehouse.Configurations;
using Warehouse.Models;

namespace Warehouse;

public class WarehouseContext : DbContext
{
    public virtual DbSet<Pallet> Pallets { get; set; }
    public virtual DbSet<Box> Boxes { get; set; }

    public WarehouseContext()
    {
    }
    
    public WarehouseContext(DbContextOptions<WarehouseContext> options) : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=warehouse;Username=warehouse;Password=password");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new PalletEntityTypeConfiguration().Configure(modelBuilder.Entity<Pallet>());
        new BoxEntityTypeConfiguration().Configure(modelBuilder.Entity<Box>());
 
        base.OnModelCreating(modelBuilder);
    }

}
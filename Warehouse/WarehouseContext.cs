using Microsoft.EntityFrameworkCore;
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
        modelBuilder.Entity<Pallet>()
            .HasKey(p => p.Id);
        
        modelBuilder.Entity<Pallet>()
            .HasMany(p => p.Boxes)
            .WithOne(b => b.Pallet)
            .HasForeignKey(b => b.PalletId);
        
        modelBuilder.Entity<Box>()
            .HasKey(b => b.Id);

        modelBuilder.Entity<Box>()
            .HasOne(b => b.Pallet)
            .WithMany(p => p.Boxes)
            .HasForeignKey(b => b.PalletId)
            .IsRequired();
        
        base.OnModelCreating(modelBuilder);
    }

}
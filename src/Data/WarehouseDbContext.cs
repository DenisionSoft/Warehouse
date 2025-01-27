using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Configurations;
using Warehouse.Domain.Entities;

namespace Warehouse.Data;

public sealed class WarehouseDbContext : DbContext
{
    public DbSet<Pallet> Pallets { get; set; }
    public DbSet<Box> Boxes { get; set; }

    public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new PalletEntityTypeConfiguration().Configure(modelBuilder.Entity<Pallet>());
        new BoxEntityTypeConfiguration().Configure(modelBuilder.Entity<Box>());

        base.OnModelCreating(modelBuilder);
    }

}

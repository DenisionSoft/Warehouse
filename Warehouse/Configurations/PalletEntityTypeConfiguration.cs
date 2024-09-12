using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.Models;

namespace Warehouse.Configurations;

public class PalletEntityTypeConfiguration : IEntityTypeConfiguration<Pallet>
{
    public void Configure(EntityTypeBuilder<Pallet> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasMany(p => p.Boxes).WithOne(b => b.Pallet).HasForeignKey(b => b.PalletId);
    }
}
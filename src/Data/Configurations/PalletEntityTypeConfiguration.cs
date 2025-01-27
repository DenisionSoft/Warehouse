using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.Domain.Entities;

namespace Warehouse.Data.Configurations;

public sealed class PalletEntityTypeConfiguration : IEntityTypeConfiguration<Pallet>
{
    public void Configure(EntityTypeBuilder<Pallet> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasMany(p => p.Boxes).WithOne(b => b.Pallet).HasForeignKey(b => b.PalletId);
    }
}
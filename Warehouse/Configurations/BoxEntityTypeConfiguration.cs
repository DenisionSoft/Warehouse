using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.Models;

namespace Warehouse.Configurations;

public class BoxEntityTypeConfiguration : IEntityTypeConfiguration<Box>
{
    public void Configure(EntityTypeBuilder<Box> builder)
    {
        builder.HasKey(b => b.Id);
        builder.HasOne(b => b.Pallet).WithMany(p => p.Boxes).HasForeignKey(b => b.PalletId).IsRequired();
    }
}
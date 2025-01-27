namespace Warehouse.Web.Contracts.Models.Pallet;

public sealed class UpdatePalletRequest
{
    public double? Width { get; set; }

    public double? Height { get; set; }

    public double? Length { get; set; }
}

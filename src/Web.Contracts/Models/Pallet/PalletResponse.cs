using Warehouse.Web.Contracts.Models.Box;

namespace Warehouse.Web.Contracts.Models.Pallet;

public sealed class PalletResponse
{
    public long Id { get; set; }

    public double Width { get; set; }

    public double Height { get; set; }

    public double Length { get; set; }

    public double Weight { get; set; }

    public double Volume { get; set; }

    public DateOnly? ExpirationDate { get; set; }

    public List<BoxResponse> Boxes { get; set; } = new List<BoxResponse>();
}

namespace Warehouse.Web.Contracts.Models.Box;

public sealed class BoxResponse
{
    public long Id { get; set; }

    public double Width { get; set; }

    public double Height { get; set; }

    public double Length { get; set; }

    public double Weight { get; set; }

    public double Volume { get; set; }

    public DateOnly? ProductionDate { get; set; }

    public DateOnly? ExpirationDate { get; set; }

    public long  PalletId { get; set; }
}

namespace Warehouse.Web.Contracts.Models.Box;

public sealed class CreateBoxRequest
{
    public double? Width { get; set; }

    public double? Height { get; set; }

    public double? Length { get; set; }

    public double? Weight { get; set; }

    public DateOnly? GivenDate { get; set; }

    public bool? IsExpirationDate { get; set; }
}

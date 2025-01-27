namespace Warehouse.Web.Contracts.Models;

public sealed class PaginationParams
{
    public int? Offset { get; set; }

    public int? Limit { get; set; }
}

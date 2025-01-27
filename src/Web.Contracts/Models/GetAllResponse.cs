namespace Warehouse.Web.Contracts.Models;

public sealed class GetAllResponse<T>
{
    public List<T> Items { get; set; } = new List<T>();

    public long Total { get; set; }

    public GetAllResponse()
    {
    }

    public GetAllResponse(List<T> items, long total)
    {
        Items = items;
        Total = total;
    }
}

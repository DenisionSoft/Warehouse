using System.ComponentModel.DataAnnotations;

namespace Warehouse.Web.GeneratedClient.Options;

public sealed class WarehouseClientOptions
{
    public static readonly string OptionKey = "WarehouseClient";

    [Required]
    public Uri? ServerUrl { get; set; }
}

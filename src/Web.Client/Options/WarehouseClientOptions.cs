using System.ComponentModel.DataAnnotations;

namespace Warehouse.Web.Client.Options;

public sealed class WarehouseClientOptions
{
    public static readonly string OptionKey = "WarehouseClient";

    [Required]
    public Uri? ServerUrl { get; set; }
}

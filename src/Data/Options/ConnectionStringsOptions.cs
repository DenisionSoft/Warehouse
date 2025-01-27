using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Options;

public sealed class ConnectionStringsOptions
{
    public static readonly string OptionKey = "ConnectionStrings";

    [Required]
    public string? DbConnection { get; set; }
}

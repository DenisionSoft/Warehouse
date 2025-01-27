using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Options;

public sealed class DatabaseProviderOptions
{
    public static readonly string OptionKey = "Database";

    public enum DataProvider
    {
        Sqlite,
        Psql
    }

    [Required]
    public DataProvider? Provider { get; set; }
}

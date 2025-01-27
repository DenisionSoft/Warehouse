using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Warehouse.Business.Boxes;
using Warehouse.Business.Pallets;
using Warehouse.Data.Options;
using Warehouse.Data.Repositories;

namespace Warehouse.Data;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IPalletRepository, PalletRepository>();
        services.AddTransient<IBoxRepository, BoxRepository>();
        return services;
    }

    public static IServiceCollection AddDb(this IServiceCollection services)
    {
        services.AddDatabaseOptions();

        services.AddDbContext<WarehouseDbContext>((provider, builder) =>
        {
            var providerOptions = provider.GetRequiredService<IOptions<DatabaseProviderOptions>>().Value;
            var connectionOptions = provider.GetRequiredService<IOptions<ConnectionStringsOptions>>().Value;

            switch (providerOptions.Provider)
            {
                case DatabaseProviderOptions.DataProvider.Sqlite:
                    builder.UseSqlite(connectionOptions.DbConnection, b => b.MigrationsAssembly("Data.Migrations.Sqlite"));
                    break;
                case DatabaseProviderOptions.DataProvider.Psql:
                    builder.UseNpgsql(connectionOptions.DbConnection, b => b.MigrationsAssembly("Data.Migrations.Psql"));
                    break;
            }
        });

        return services;
    }

    private static IServiceCollection AddDatabaseOptions(this IServiceCollection services)
    {
        services.AddOptions<DatabaseProviderOptions>()
            .BindConfiguration(DatabaseProviderOptions.OptionKey)
            .ValidateDataAnnotations();

        services.AddOptions<ConnectionStringsOptions>()
            .BindConfiguration(ConnectionStringsOptions.OptionKey)
            .ValidateDataAnnotations();

        return services;
    }
}

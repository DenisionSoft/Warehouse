using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Warehouse.Web.Client.Boxes;
using Warehouse.Web.Client.Options;
using Warehouse.Web.Client.Pallets;

namespace Warehouse.Web.Client;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddClients(this IServiceCollection services)
    {
        services.AddClientOptions();

        services.AddHttpClient<IPalletClient, PalletClient>(
            (serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<WarehouseClientOptions>>().Value;
                client.BaseAddress = options.ServerUrl;
            });

        services.AddHttpClient<IBoxClient, BoxClient>(
            (serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<WarehouseClientOptions>>().Value;
                client.BaseAddress = options.ServerUrl;
            });

        return services;
    }

    private static IServiceCollection AddClientOptions(this IServiceCollection services)
    {
        services.AddOptions<WarehouseClientOptions>()
            .BindConfiguration(WarehouseClientOptions.OptionKey)
            .ValidateDataAnnotations();

        return services;
    }
}

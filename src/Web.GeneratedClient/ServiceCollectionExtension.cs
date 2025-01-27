using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Warehouse.Web.GeneratedClient.Options;

namespace Warehouse.Web.GeneratedClient;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddGeneratedClient(this IServiceCollection services)
    {
        services.AddClientOptions();

        services.AddHttpClient<IGeneratedClient, GeneratedClient>((provider, client) =>
        {
            var baseUrl = provider.GetRequiredService<IOptions<WarehouseClientOptions>>().Value.ServerUrl;
            client.BaseAddress = baseUrl;
        });


        return services;
    }

    private static IServiceCollection AddClientOptions(this IServiceCollection service)
    {
        service.AddOptions<WarehouseClientOptions>()
            .BindConfiguration(WarehouseClientOptions.OptionKey)
            .ValidateDataAnnotations();

        return service;
    }
}

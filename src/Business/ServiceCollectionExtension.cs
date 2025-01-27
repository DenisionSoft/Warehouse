using Warehouse.Business.Boxes;
using Warehouse.Business.Pallets;
using Microsoft.Extensions.DependencyInjection;

namespace Warehouse.Business;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddBusiness(this IServiceCollection services)
    {
        services.AddTransient<IPalletService, PalletService>();
        services.AddTransient<IBoxService, BoxService>();
        return services;
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Warehouse.Web.Client;

namespace Warehouse.ConsoleApplication;

class Program
{
    public static Task Main()
    {

        var builder = Host.CreateEmptyApplicationBuilder(null);

        builder.Configuration.AddJsonFile("appsettings.json");
        builder.Services.AddClients();
        builder.Services.AddSingleton<ConsoleApplication>();

        var app = builder.Build().Services.GetRequiredService<ConsoleApplication>();
        return app.RunAsync();
    }
}

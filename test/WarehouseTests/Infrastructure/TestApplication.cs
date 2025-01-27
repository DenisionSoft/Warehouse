using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Warehouse.Data;
using Warehouse.Data.Options;
using Warehouse.Web;

namespace Warehouse.WarehouseTests.Infrastructure;

public sealed class TestApplication : WebApplicationFactory<IWebMarker>, IAsyncLifetime
{
    public HttpClient HttpClient;
    private WarehouseDbContext dbContext;

    private readonly PostgreSqlContainer dbContainer =
        new PostgreSqlBuilder()
            .WithImage(DockerImages.PostgreSql)
            .WithDatabase("WarehouseTestDb")
            .WithUsername("warehouse")
            .WithPassword("password")
            .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .UseSetting($"{ConnectionStringsOptions.OptionKey}:{nameof(ConnectionStringsOptions.DbConnection)}", dbContainer.GetConnectionString())
            .UseSetting($"{DatabaseProviderOptions.OptionKey}:{nameof(DatabaseProviderOptions.Provider)}", "Psql");
    }

    public async Task InitializeAsync()
    {
        await dbContainer.StartAsync();
        dbContext = Services.CreateScope().ServiceProvider.GetRequiredService<WarehouseDbContext>();
        HttpClient = CreateClient();
    }

    public new Task DisposeAsync()
    {
        return dbContainer.StopAsync();
    }
}

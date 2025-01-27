using AutoFixture;
using Warehouse.WarehouseTests.Infrastructure;
using Warehouse.Web.Client.Boxes;
using Warehouse.Web.Client.Pallets;
using Warehouse.Web.Contracts.Models.Box;

namespace Warehouse.WarehouseTests.Abstract;

[Trait("Category", IntegrationTestCollection.Category)]
[Collection(IntegrationTestCollection.Name)]
public abstract class ControllerTestsBase : IAsyncLifetime
{
    protected readonly IFixture Fixture = new Fixture();

    protected readonly IPalletClient PalletClient;
    protected readonly IBoxClient BoxClient;

    protected ControllerTestsBase(TestApplication testApplication)
    {
        PalletClient = new PalletClient(testApplication.HttpClient);
        BoxClient = new BoxClient(testApplication.HttpClient);
    }

    public Task InitializeAsync()
    {
        Fixture.Customize<DateOnly>(o => o.FromFactory((DateTime dt) => DateOnly.FromDateTime(dt)));
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    protected CreateBoxRequest BuildValidCreateBoxRequestForPalletDimensions(double palletWidth, double palletLength)
    {
        return Fixture.Build<CreateBoxRequest>()
            .With(r => r.Width, palletWidth)
            .With(r => r.Length, palletLength)
            .Create();
    }
}

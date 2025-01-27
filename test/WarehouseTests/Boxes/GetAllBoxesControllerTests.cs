using AutoFixture;
using FluentAssertions;
using Warehouse.WarehouseTests.Abstract;
using Warehouse.WarehouseTests.Infrastructure;
using Warehouse.Web.Contracts.Models;
using Warehouse.Web.Contracts.Models.Pallet;

namespace Warehouse.WarehouseTests.Boxes;

public sealed class GetAllBoxesControllerTests : ControllerTestsBase
{

    public GetAllBoxesControllerTests(TestApplication testApplication) : base(testApplication)
    {
    }

    [Fact]
    public async Task GetAll_ReturnsBoxesList_WithExistingBoxes()
    {
        // Arrange
        var pallet = (await PalletClient.AddPallet(Fixture.Create<CreatePalletRequest>())).ShouldNotBeNull();
        var request1 = BuildValidCreateBoxRequestForPalletDimensions(pallet.Width, pallet.Length);
        var request2 = BuildValidCreateBoxRequestForPalletDimensions(pallet.Width, pallet.Length);
        var request3 = BuildValidCreateBoxRequestForPalletDimensions(pallet.Width, pallet.Length);
        await BoxClient.AddBox(pallet.Id, request1);
        await BoxClient.AddBox(pallet.Id, request2);
        await BoxClient.AddBox(pallet.Id, request3);

        var paginationParams = new PaginationParams { Limit = 3, Offset = 0 };

        // Act
        var getResponse = await BoxClient.GetAllBoxes(paginationParams);

        // Assert
        getResponse.Total.Should().BeGreaterOrEqualTo(3);
        getResponse.Items.Should().HaveCount(3);
    }
}

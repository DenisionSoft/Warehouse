using System.Net;
using AutoFixture;
using FluentAssertions;
using Warehouse.WarehouseTests.Abstract;
using Warehouse.WarehouseTests.Infrastructure;
using Warehouse.Web.Contracts.Models.Pallet;

namespace Warehouse.WarehouseTests.Boxes;

public sealed class GetBoxControllerTests : ControllerTestsBase
{

    public GetBoxControllerTests(TestApplication testApplication) : base(testApplication)
    {
    }

    [Fact]
    public async Task Get_ReturnsBox_WhenBoxExists()
    {
        // Arrange
        var pallet = (await PalletClient.AddPallet(Fixture.Create<CreatePalletRequest>())).ShouldNotBeNull();
        var request = BuildValidCreateBoxRequestForPalletDimensions(pallet.Width, pallet.Length);
        var createResponse = (await BoxClient.AddBox(pallet.Id, request)).ShouldNotBeNull();

        // Act
        var getResponse = await BoxClient.GetBoxById(createResponse.Id);

        // Assert
        getResponse.Should().BeEquivalentTo(createResponse);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenPalletDoesNotExist()
    {
        // Act
        Func<Task> act = () => BoxClient.GetBoxById(-1);

        // Assert
        var exception = await act.Should().ThrowAsync<HttpRequestException>();
        exception.And.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}

using System.Net;
using AutoFixture;
using FluentAssertions;
using Warehouse.WarehouseTests.Abstract;
using Warehouse.WarehouseTests.Infrastructure;
using Warehouse.Web.Contracts.Models.Pallet;

namespace Warehouse.WarehouseTests.Pallets;

public sealed class UpdatePalletControllerTests : ControllerTestsBase
{

    public UpdatePalletControllerTests(TestApplication testApplication) : base(testApplication)
    {
    }

    [Fact]
    public async Task Update_UpdatesPallet_WhenRequestIsValid()
    {
        // Arrange
        var pallet = (await PalletClient.AddPallet(Fixture.Create<CreatePalletRequest>())).ShouldNotBeNull();
        var request = Fixture.Create<UpdatePalletRequest>();

        // Act
        var response = await PalletClient.UpdatePallet(pallet.Id, request);

        // Assert
        response.Should().NotBeNull();
        response.Should().BeEquivalentTo(request);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenPalletDoesNotExist()
    {
        // Arrange
        var request = Fixture.Create<UpdatePalletRequest>();

        // Act
        Func<Task> act = () => PalletClient.UpdatePallet(-1, request);

        // Assert
        var exception = await act.Should().ThrowAsync<HttpRequestException>();
        exception.And.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}

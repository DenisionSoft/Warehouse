using System.Net;
using AutoFixture;
using FluentAssertions;
using Warehouse.WarehouseTests.Abstract;
using Warehouse.WarehouseTests.Infrastructure;
using Warehouse.Web.Contracts.Models.Pallet;

namespace Warehouse.WarehouseTests.Pallets;

public sealed class GetPalletControllerTests : ControllerTestsBase
{

    public GetPalletControllerTests(TestApplication testApplication) : base(testApplication)
    {
    }

    [Fact]
    public async Task Get_ReturnsPallet_WhenPalletExists()
    {
        // Arrange
        var request = Fixture.Create<CreatePalletRequest>();
        var createResponse = (await PalletClient.AddPallet(request)).ShouldNotBeNull();

        // Act
        var getResponse = await PalletClient.GetPalletById(createResponse.Id);

        // Assert
        getResponse.Should().BeEquivalentTo(createResponse);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenPalletDoesNotExist()
    {
        // Act
        Func<Task> act = () => PalletClient.GetPalletById(-1);

        // Assert
        var exception = await act.Should().ThrowAsync<HttpRequestException>();
        exception.And.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}

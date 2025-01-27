using System.Net;
using AutoFixture;
using FluentAssertions;
using Warehouse.WarehouseTests.Abstract;
using Warehouse.WarehouseTests.Infrastructure;
using Warehouse.Web.Contracts.Models.Pallet;

namespace Warehouse.WarehouseTests.Pallets;

public sealed class AddPalletControllerTests : ControllerTestsBase
{

    public AddPalletControllerTests(TestApplication testApplication) : base(testApplication)
    {
    }

    [Fact]
    public async Task Add_AddsPallet_WhenRequestIsValid()
    {
        // Arrange
        var request = Fixture.Create<CreatePalletRequest>();

        // Act
        var response = await PalletClient.AddPallet(request);

        // Assert
        response.Should().NotBeNull();
        response.Should().BeEquivalentTo(request);
    }

    [Fact]
    public async Task Add_ReturnsBadRequest_WhenRequestIsInvalid()
    {
        // Arrange
        var request = Fixture.Create<CreatePalletRequest>();
        request.Height = null;

        // Act
        Func<Task> act = () => PalletClient.AddPallet(request);

        // Assert
        var exception = await act.Should().ThrowAsync<HttpRequestException>();
        exception.And.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}

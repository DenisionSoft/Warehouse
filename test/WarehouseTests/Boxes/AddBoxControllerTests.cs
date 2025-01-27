using System.Net;
using AutoFixture;
using FluentAssertions;
using Warehouse.WarehouseTests.Abstract;
using Warehouse.WarehouseTests.Infrastructure;
using Warehouse.Web.Contracts.Models.Pallet;

namespace Warehouse.WarehouseTests.Boxes;

public sealed class AddBoxControllerTests : ControllerTestsBase
{

    public AddBoxControllerTests(TestApplication testApplication) : base(testApplication)
    {
    }

    [Fact]
    public async Task Add_AddsBox_WhenRequestIsValid()
    {
        // Arrange
        var pallet = (await PalletClient.AddPallet(Fixture.Create<CreatePalletRequest>())).ShouldNotBeNull();
        var request = BuildValidCreateBoxRequestForPalletDimensions(pallet.Width, pallet.Length);

        // Act
        var response = await BoxClient.AddBox(pallet.Id, request);

        // Assert
        response.Should().NotBeNull();
        response.Should().BeEquivalentTo(request, options =>
            options.Excluding(r => r.GivenDate).Excluding(r => r.IsExpirationDate));
    }

    [Fact]
    public async Task Add_ReturnsBadRequest_WhenRequestIsInvalid()
    {
        // Arrange
        var pallet = (await PalletClient.AddPallet(Fixture.Create<CreatePalletRequest>())).ShouldNotBeNull();
        var request = BuildValidCreateBoxRequestForPalletDimensions(pallet.Width, pallet.Length);
        request.Height = null;

        // Act
        Func<Task> act = () => BoxClient.AddBox(pallet.Id, request);

        // Assert
        var exception = await act.Should().ThrowAsync<HttpRequestException>();
        exception.And.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Add_ReturnsNotFound_WhenPalletDoesNotExist()
    {
        // Arrange
        var request = BuildValidCreateBoxRequestForPalletDimensions(10, 10);

        // Act
        Func<Task> act = () => BoxClient.AddBox(-1, request);

        // Assert
        var exception = await act.Should().ThrowAsync<HttpRequestException>();
        exception.And.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}

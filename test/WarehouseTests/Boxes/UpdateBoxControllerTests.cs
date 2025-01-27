using System.Net;
using AutoFixture;
using FluentAssertions;
using Warehouse.WarehouseTests.Abstract;
using Warehouse.WarehouseTests.Infrastructure;
using Warehouse.Web.Contracts.Models.Box;
using Warehouse.Web.Contracts.Models.Pallet;

namespace Warehouse.WarehouseTests.Boxes;

public sealed class UpdateBoxControllerTests : ControllerTestsBase
{

    public UpdateBoxControllerTests(TestApplication testApplication) : base(testApplication)
    {
    }

    [Fact]
    public async Task Update_UpdatesBox_WhenRequestIsValid()
    {
        // Arrange
        var pallet = (await PalletClient.AddPallet(Fixture.Create<CreatePalletRequest>())).ShouldNotBeNull();
        var box = (await BoxClient.AddBox(pallet.Id, BuildValidCreateBoxRequestForPalletDimensions(pallet.Width, pallet.Length))).ShouldNotBeNull();
        var request = BuildValidUpdateBoxRequestForPallet(pallet.Id, pallet.Width, pallet.Length);

        // Act
        var response = await BoxClient.UpdateBox(pallet.Id, box.Id, request);

        // Assert
        response.Should().NotBeNull();
        response.Should().BeEquivalentTo(request);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenBoxDoesNotExist()
    {
        // Arrange
        var pallet = (await PalletClient.AddPallet(Fixture.Create<CreatePalletRequest>())).ShouldNotBeNull();
        var request = BuildValidUpdateBoxRequestForPallet(pallet.Id, pallet.Width, pallet.Length);

        // Act
        Func<Task> act = () => BoxClient.UpdateBox(pallet.Id, -1, request);

        // Assert
        var exception = await act.Should().ThrowAsync<HttpRequestException>();
        exception.And.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenPalletDoesNotExist()
    {
        // Arrange
        var pallet = (await PalletClient.AddPallet(Fixture.Create<CreatePalletRequest>())).ShouldNotBeNull();
        var box = (await BoxClient.AddBox(pallet.Id, BuildValidCreateBoxRequestForPalletDimensions(pallet.Width, pallet.Length))).ShouldNotBeNull();
        var request = BuildValidUpdateBoxRequestForPallet(pallet.Id, pallet.Width, pallet.Length);

        // Act
        Func<Task> act = () => BoxClient.UpdateBox(-1, box.Id, request);

        // Assert
        var exception = await act.Should().ThrowAsync<HttpRequestException>();
        exception.And.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenBoxDoesNotBelongToPallet()
    {
        // Arrange
        var pallet1 = (await PalletClient.AddPallet(Fixture.Create<CreatePalletRequest>())).ShouldNotBeNull();
        var pallet2 = (await PalletClient.AddPallet(Fixture.Create<CreatePalletRequest>())).ShouldNotBeNull();

        var box = (await BoxClient.AddBox(pallet1.Id, BuildValidCreateBoxRequestForPalletDimensions(pallet1.Width, pallet1.Length))).ShouldNotBeNull();
        var request = BuildValidUpdateBoxRequestForPallet(pallet1.Id, pallet1.Width, pallet1.Length);

        // Act
        Func<Task> act = () => BoxClient.UpdateBox(pallet2.Id, box.Id, request);

        // Assert
        var exception = await act.Should().ThrowAsync<HttpRequestException>();
        exception.And.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    private UpdateBoxRequest BuildValidUpdateBoxRequestForPallet(long palletId, double palletWidth, double palletLength)
    {
        return Fixture.Build<UpdateBoxRequest>()
            .With(r => r.Width, palletWidth)
            .With(r => r.Length, palletLength)
            .With(r => r.PalletId, palletId)
            .With(r => r.ExpirationDate, new DateOnly(2024, 03, 23))
            .With(r => r.ProductionDate, new DateOnly(2024, 03, 10))
            .Create();
    }
}

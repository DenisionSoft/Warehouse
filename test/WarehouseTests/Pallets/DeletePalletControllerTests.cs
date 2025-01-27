using System.Net;
using AutoFixture;
using FluentAssertions;
using Warehouse.WarehouseTests.Abstract;
using Warehouse.WarehouseTests.Infrastructure;
using Warehouse.Web.Contracts.Models.Pallet;

namespace Warehouse.WarehouseTests.Pallets;

public sealed class DeletePalletControllerTests : ControllerTestsBase
{

    public DeletePalletControllerTests(TestApplication testApplication) : base(testApplication)
    {
    }

    [Fact]
    public async Task Delete_DeletesPallet_WhenPalletExists()
    {
        // Arrange
        var pallet = (await PalletClient.AddPallet(Fixture.Create<CreatePalletRequest>())).ShouldNotBeNull();

        // Act
        await PalletClient.DeletePallet(pallet.Id);

        // Assert
        Func<Task> act = () => PalletClient.GetPalletById(pallet.Id);
        var exception = await act.Should().ThrowAsync<HttpRequestException>();
        exception.And.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_FinishesSuccessfully_WhenPalletDoesNotExist()
    {
        // Act
        Func<Task> act = () => PalletClient.DeletePallet(-1);

        // Assert
        await act.Should().NotThrowAsync();
    }
}

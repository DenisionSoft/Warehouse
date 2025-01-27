using System.Net;
using AutoFixture;
using FluentAssertions;
using Warehouse.WarehouseTests.Abstract;
using Warehouse.WarehouseTests.Infrastructure;
using Warehouse.Web.Contracts.Models.Pallet;

namespace Warehouse.WarehouseTests.Boxes;

public sealed class DeleteBoxControllerTests : ControllerTestsBase
{

    public DeleteBoxControllerTests(TestApplication testApplication) : base(testApplication)
    {
    }

    [Fact]
    public async Task Delete_DeletesBox_WhenBoxExists()
    {
        // Arrange
        var pallet = (await PalletClient.AddPallet(Fixture.Create<CreatePalletRequest>())).ShouldNotBeNull();
        var box = (await BoxClient.AddBox(pallet.Id, BuildValidCreateBoxRequestForPalletDimensions(pallet.Width, pallet.Length))).ShouldNotBeNull();

        // Act
        await BoxClient.DeleteBox(box.Id);

        // Assert
        Func<Task> act = () => BoxClient.GetBoxById(box.Id);
        var exception = await act.Should().ThrowAsync<HttpRequestException>();
        exception.And.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_FinishesSuccessfully_WhenBoxDoesNotExist()
    {
        // Act
        Func<Task> act = () => BoxClient.DeleteBox(-1);

        // Assert
        await act.Should().NotThrowAsync();
    }
}

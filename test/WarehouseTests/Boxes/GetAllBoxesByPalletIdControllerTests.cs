using System.Net;
using AutoFixture;
using FluentAssertions;
using Warehouse.WarehouseTests.Abstract;
using Warehouse.WarehouseTests.Infrastructure;
using Warehouse.Web.Contracts.Models.Box;
using Warehouse.Web.Contracts.Models.Pallet;

namespace Warehouse.WarehouseTests.Boxes;

public sealed class GetAllBoxesByPalletIdControllerTests : ControllerTestsBase
{

    public GetAllBoxesByPalletIdControllerTests(TestApplication testApplication) : base(testApplication)
    {
    }

    [Fact]
    public async Task GetAllById_ReturnsBoxesList_WithExistingBoxes()
    {
        // Arrange
        var pallet = (await PalletClient.AddPallet(Fixture.Create<CreatePalletRequest>())).ShouldNotBeNull();
        var request1 = BuildValidCreateBoxRequestForPalletDimensions(pallet.Width, pallet.Length);
        var request2 = BuildValidCreateBoxRequestForPalletDimensions(pallet.Width, pallet.Length);
        var request3 = BuildValidCreateBoxRequestForPalletDimensions(pallet.Width, pallet.Length);
        var requestList = new List<CreateBoxRequest> { request1, request2, request3 };
        await BoxClient.AddBox(pallet.Id, request1);
        await BoxClient.AddBox(pallet.Id, request2);
        await BoxClient.AddBox(pallet.Id, request3);

        // Act
        var getResponse = await BoxClient.GetAllBoxesByPalletId(pallet.Id);

        // Assert
        getResponse.Total.Should().BeGreaterOrEqualTo(3);
        getResponse.Items.Should().HaveCount(3);
        getResponse.Items.Should().BeEquivalentTo(requestList, options =>
            options.Excluding(r => r.GivenDate).Excluding(r => r.IsExpirationDate));
    }

    [Fact]
    public async Task GetAllById_ReturnsNotFound_WhenPalletDoesNotExist()
    {
        // Act
        Func<Task> act = () => BoxClient.GetAllBoxesByPalletId(-1);

        // Assert
        var exception = await act.Should().ThrowAsync<HttpRequestException>();
        exception.And.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAllById_ReturnsEmptyList_WhenPalletHasNoBoxes()
    {
        // Arrange
        var pallet = (await PalletClient.AddPallet(Fixture.Create<CreatePalletRequest>())).ShouldNotBeNull();

        // Act
        var getResponse = await BoxClient.GetAllBoxesByPalletId(pallet.Id);

        // Assert
        getResponse.Total.Should().Be(0);
        getResponse.Items.Should().BeEmpty();
    }

}

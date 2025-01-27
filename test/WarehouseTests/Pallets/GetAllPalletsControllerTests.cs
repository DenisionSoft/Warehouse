using System.Net;
using AutoFixture;
using FluentAssertions;
using Warehouse.WarehouseTests.Abstract;
using Warehouse.WarehouseTests.Infrastructure;
using Warehouse.Web.Contracts.Models;
using Warehouse.Web.Contracts.Models.Pallet;

namespace Warehouse.WarehouseTests.Pallets;

public sealed class GetAllPalletsControllerTests : ControllerTestsBase
{

    public GetAllPalletsControllerTests(TestApplication testApplication) : base(testApplication)
    {
    }

    [Fact]
    public async Task GetAll_ReturnsPalletsList_WithExistingPallets()
    {
        // Arrange
        var request1 = Fixture.Create<CreatePalletRequest>();
        var request2 = Fixture.Create<CreatePalletRequest>();
        var request3 = Fixture.Create<CreatePalletRequest>();
        await PalletClient.AddPallet(request1);
        await PalletClient.AddPallet(request2);
        await PalletClient.AddPallet(request3);

        var paginationParams = new PaginationParams { Limit = 3, Offset = 0 };

        // Act
        var getResponse = await PalletClient.GetAllPallets(paginationParams);

        // Assert
        getResponse.Total.Should().BeGreaterOrEqualTo(3);
        getResponse.Items.Should().HaveCount(3);
    }

    [Theory]
    [MemberData(nameof(InvalidPaginationParams))]
    public async Task GetAll_ReturnsBadRequest_WhenPaginationParamsInvalid(int offset, int limit)
    {
        // Arrange
        var paginationParams = new PaginationParams { Limit = limit, Offset = offset };

        // Act
        Func<Task> act = () => PalletClient.GetAllPallets(paginationParams);

        // Assert
        var exception = await act.Should().ThrowAsync<HttpRequestException>();
        exception.And.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    public static IEnumerable<object[]> InvalidPaginationParams =>
        new List<object[]>
        {
            new object[] {-1, 1},
            new object[] {0, -1},
            new object[] {0, 0},
            new object[] {-1, -1},
        };
}

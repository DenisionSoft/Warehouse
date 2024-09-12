using Xunit;
using Moq;
using FluentAssertions;
using Warehouse;
using Warehouse.Models;
using Warehouse.Repositories;
using Warehouse.Services;
using Warehouse.Utils;
using Xunit.Abstractions;

namespace WarehouseTests;

public class PalletServiceTests
{
    private readonly Mock<WarehouseContext> _mockContext;
    private readonly PalletService _palletService;
    private readonly Mock<PalletsRepository> _mockRepo;

    public PalletServiceTests()
    {
        _mockContext = MockWarehouseContext.GetMockedContext();
        _mockRepo = new Mock<PalletsRepository>(_mockContext.Object);
        _palletService = new PalletService(_mockRepo.Object);
    }
    
    [Fact]
    public async Task AddAsync_Should_Add_Pallet()
    {
        // Arrange
        var newPalletDto = new NewPalletDto(5, 6, 7);

        // Act
        await _palletService.AddAsync(newPalletDto);

        // Assert
        _mockRepo.Verify(r => r.AddAsync(It.IsAny<Pallet>()), Times.Once);
    }

    [Fact]
    public async Task AddRangeAsync_Should_Add_Range_Of_Pallets()
    {
        // Arrange
        var pallet1 = new Pallet(1, 1, 1);
        var pallet2 = new Pallet(2, 2, 2);
        var pallet3 = new Pallet(3, 3, 3);
        var pallets = new List<Pallet> { pallet1, pallet2, pallet3 };

        // Act
        await _palletService.AddRangeAsync(pallets);

        // Assert
        _mockRepo.Verify(r => r.AddRangeAsync(It.IsAny<List<Pallet>>()), Times.Once);
    }
    
    
}
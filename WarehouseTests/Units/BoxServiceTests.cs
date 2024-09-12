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

public class BoxServiceTests
{
    private readonly Mock<WarehouseContext> _mockContext;
    private readonly BoxService _boxService;
    private readonly Mock<PalletsRepository> _mockPalletsRepo;
    private readonly Mock<BoxesRepository> _mockBoxesRepo;

    public BoxServiceTests()
    {
        _mockContext = MockWarehouseContext.GetMockedContext();
        _mockPalletsRepo = new Mock<PalletsRepository>(_mockContext.Object);
        _mockBoxesRepo = new Mock<BoxesRepository>(_mockContext.Object);
        _boxService = new BoxService(_mockBoxesRepo.Object, _mockPalletsRepo.Object);
    }
    
    [Fact]
    public async Task AddAsync_Should_Add_Box()
    {
        // Arrange
        var newBoxDto = new NewBoxDto(1, 1, 1, 1, DateOnly.MinValue, true, 1);
        _mockPalletsRepo.Setup(r => r.GetById(1)).ReturnsAsync(new Pallet(10, 10, 10) {Id = 1});
        
        // Act
        await _boxService.AddAsync(newBoxDto);
        
        // Assert
        _mockBoxesRepo.Verify(r => r.AddAsync(It.IsAny<Box>()), Times.Once);
    }
    
    [Fact]
    public async Task AddAsync_Should_Reject_When_Pallet_Not_Exists()
    {
        // Arrange
        var newBoxDto = new NewBoxDto(1, 1, 1, 1, DateOnly.MinValue, true, 1);
        _mockPalletsRepo.Setup(r => r.GetById(1)).ReturnsAsync((Pallet?) null);
        
        // Act
        Func<Task> act = async () => await _boxService.AddAsync(newBoxDto);
        
        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task AddAsync_Should_Reject_When_Box_Too_Big()
    {
        // Arrange
        var newBoxDto = new NewBoxDto(11, 1, 1, 1, DateOnly.MinValue, true, 1);
        _mockPalletsRepo.Setup(r => r.GetById(1)).ReturnsAsync(new Pallet(10, 10, 10) { Id = 1 });

        // Act
        Func<Task> act = async () => await _boxService.AddAsync(newBoxDto);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }
}
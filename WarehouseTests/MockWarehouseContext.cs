using Moq;
using Warehouse.Repositories;
using Warehouse.Models;
using Microsoft.EntityFrameworkCore;
using Warehouse;

namespace WarehouseTests;

public static class MockWarehouseContext
{
    public static Mock<WarehouseContext> GetMockedContext()
    {
        var pallet1 = new Pallet(2.5, 3.0, 4.0)
        {
            Id = 1, Boxes = new List<Box>
            {
                new()
                {
                    Id = 1, Width = 1.2, Height = 1.5, Length = 2.0, Weight = 10,
                    ExpirationDate = new DateOnly(2024, 12, 10), PalletId = 1
                },
                new()
                {
                    Id = 2, Width = 1.5, Height = 2.0, Length = 2.5, Weight = 24,
                    ExpirationDate = new DateOnly(2024, 10, 01), PalletId = 1
                }
            }
        };
        
        var pallet2 = new Pallet(3.0, 4.0, 5.0)
        {
            Id = 2, Boxes = new List<Box>
            {
                new()
                {
                    Id = 3, Width = 2.0, Height = 2.5, Length = 3.0, Weight = 12,
                    ExpirationDate = new DateOnly(2025, 01, 20), PalletId = 2
                },
                new()
                {
                    Id = 4, Width = 2.5, Height = 3.0, Length = 3.5, Weight = 29,
                    ExpirationDate = new DateOnly(2024, 09, 12), PalletId = 2
                }
            }
        };
        
        var pallet3 = new Pallet(4.0, 5.0, 6.0)
        {
            Id = 3, Boxes = new List<Box>
            {
                new()
                {
                    Id = 5, Width = 3.0, Height = 3.5, Length = 4.0, Weight = 15,
                    ExpirationDate = new DateOnly(2024, 09, 12), PalletId = 3
                }
            }
        };
        
        var mockContext = new Mock<WarehouseContext>();
        
        var palletsData = new List<Pallet>
        {
            pallet1, pallet2, pallet3
        }.AsQueryable();

        var mockPalletsSet = new Mock<DbSet<Pallet>>();
        mockPalletsSet.As<IQueryable<Pallet>>().Setup(m => m.Provider).Returns(palletsData.Provider);
        mockPalletsSet.As<IQueryable<Pallet>>().Setup(m => m.Expression).Returns(palletsData.Expression);
        mockPalletsSet.As<IQueryable<Pallet>>().Setup(m => m.ElementType).Returns(palletsData.ElementType);
        mockPalletsSet.As<IQueryable<Pallet>>().Setup(m => m.GetEnumerator()).Returns(palletsData.GetEnumerator());
        
        mockContext.Setup(c => c.Pallets).Returns(mockPalletsSet.Object);

        var boxesData = new List<Box>
        {
            pallet1.Boxes[0], pallet1.Boxes[1], pallet2.Boxes[0], pallet2.Boxes[1], pallet3.Boxes[0]
        }.AsQueryable();
        
        var mockBoxesSet = new Mock<DbSet<Box>>();
        mockBoxesSet.As<IQueryable<Box>>().Setup(m => m.Provider).Returns(boxesData.Provider);
        mockBoxesSet.As<IQueryable<Box>>().Setup(m => m.Expression).Returns(boxesData.Expression);
        mockBoxesSet.As<IQueryable<Box>>().Setup(m => m.ElementType).Returns(boxesData.ElementType);
        mockBoxesSet.As<IQueryable<Box>>().Setup(m => m.GetEnumerator()).Returns(boxesData.GetEnumerator());
        
        mockContext.Setup(c => c.Boxes).Returns(mockBoxesSet.Object);
        
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        
        return mockContext;
    }
}
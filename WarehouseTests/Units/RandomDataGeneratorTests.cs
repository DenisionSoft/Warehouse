using FluentAssertions;
using Warehouse.Models;
using Warehouse.Utils;

namespace WarehouseTests;

public class RandomDataGeneratorTests
{
    private readonly RandomDataGenerator _randomDataGenerator;
    
    public RandomDataGeneratorTests()
    {
        _randomDataGenerator = new RandomDataGenerator();
    }
    
    [Fact]
    public void GenerateBoxes_Should_Respect_Pallet_Size()
    {
        // Act
        var pallets = _randomDataGenerator.GeneratePallets(10);
        
        // Assert
        foreach (var pallet in pallets)
        {
            foreach (var box in pallet.Boxes)
            {
                box.Width.Should().BeLessOrEqualTo(pallet.Width);
                box.Length.Should().BeLessOrEqualTo(pallet.Length);
            }
        }
    }
}
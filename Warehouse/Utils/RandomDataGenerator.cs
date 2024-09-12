using Warehouse.Models;

namespace Warehouse.Utils;

public class RandomDataGenerator
{
    private readonly Random _random;

    public RandomDataGenerator()
    {
        _random = new Random();
    }

    /// <summary>
    /// Метод <c>GeneratePallets</c> генерирует случайные паллеты с коробками.
    /// </summary>
    /// <param name="count">Количество паллет для генерации.</param>
    /// <returns>Возвращает список сгенерированных паллет.</returns>
    public List<Pallet> GeneratePallets(int count)
    {
        var pallets = new List<Pallet>();
        
        for (int i = 0; i < count; i++)
        {
            var pallet = new Pallet(
                RandomDoubleInRange(1, 5),
                RandomDoubleInRange(1, 12),
                RandomDoubleInRange(1, 5)
            );
            
            int boxesCount = _random.Next(1, 10);
            pallet.Boxes = GenerateBoxes(pallet, boxesCount);
            
            pallets.Add(pallet);
        }
        
        return pallets;
    }

    private List<Box> GenerateBoxes(Pallet pallet, int count)
    {
        var boxes = new List<Box>();
        
        for (int i = 0; i < count; i++)
        {
            boxes.Add(new Box
            {
                Width = RandomDoubleInRange(0.1, pallet.Width),
                Height = RandomDoubleInRange(0.1, pallet.Height / 2),
                Length = RandomDoubleInRange(0.1, pallet.Length),
                Weight = RandomDoubleInRange(0.1, 100),
                ExpirationDate = RandomFutureDate(),
                PalletId = pallet.Id
            });
        }
        
        return boxes;
    }
    
    private double RandomDoubleInRange(double min, double max)
    {
        return Math.Round(_random.NextDouble() * (max - min) + min, 2);
    }

    private DateOnly RandomFutureDate()
    {
        int days = _random.Next(1, 100);
        return DateOnly.FromDateTime(DateTime.Today.AddDays(days));
    }
}
using Warehouse.Business.Pallets;
using Warehouse.Domain.Entities;

namespace Warehouse.ConsoleApplication.Client;

public sealed class RandomPalletGenerator
{
    private readonly Random random = new();

    /// <summary>
    /// Метод <c>GeneratePallets</c> генерирует случайные паллеты с коробками.
    /// </summary>
    /// <param name="generatedDataDto">DTO для генерации данных.</param>
    /// <returns>Возвращает список сгенерированных паллет.</returns>
    public List<Pallet> GeneratePallets(GeneratePalletsCommand generatedDataDto)
    {
        int palletsAmount = generatedDataDto.PalletsAmount;

        var pallets = new List<Pallet>();

        for (int i = 0; i < palletsAmount; i++)
        {
            var pallet = Pallet.Create(
                RandomDoubleInRange(1, 5),
                RandomDoubleInRange(1, 12),
                RandomDoubleInRange(1, 5)
            );

            int boxesAmount = random.Next(1, 10);

            pallet.SetBoxes(GenerateBoxes(pallet, boxesAmount));

            pallets.Add(pallet);
        }

        return pallets;
    }

    private List<Box> GenerateBoxes(Pallet pallet, int amount)
    {
        var boxes = new List<Box>();

        for (int i = 0; i < amount; i++)
        {
            var box = Box.Create(
                width: RandomDoubleInRange(0.1, pallet.Width),
                height: RandomDoubleInRange(0.1, pallet.Height / 2),
                length: RandomDoubleInRange(0.1, pallet.Length),
                weight: RandomDoubleInRange(0.1, 100),
                productionDate: null,
                expirationDate: RandomFutureDate(),
                palletId: pallet.Id
            );

            boxes.Add(box);
        }

        return boxes;
    }

    private double RandomDoubleInRange(double min, double max)
    {
        return Math.Round(random.NextDouble() * (max - min) + min, 2);
    }

    private DateOnly RandomFutureDate()
    {
        int days = random.Next(1, 100);
        return DateOnly.FromDateTime(DateTime.Today.AddDays(days));
    }
}

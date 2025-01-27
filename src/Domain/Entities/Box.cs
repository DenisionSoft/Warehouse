namespace Warehouse.Domain.Entities;

public sealed class Box : Item
{
    public DateOnly? ProductionDate { get; private set; }

    public bool IsFragile { get; private set; } = false;

    public long PalletId { get; private set; }

    public Pallet? Pallet { get; private set; }

    private Box(double width, double height, double length, double weight, DateOnly? productionDate, DateOnly? expirationDate, long palletId) : base(width, height,
        length)
    {
        Weight = weight;
        ProductionDate = productionDate;
        ExpirationDate = expirationDate;
        PalletId = palletId;
    }

    public static Box Create(
        double width,
        double height,
        double length,
        double weight,
        DateOnly? productionDate,
        DateOnly expirationDate,
        long palletId)
    {
        if (!ArgumentsPositive(width, height, length, weight))
        {
            throw new ArgumentException($"Измерения коробки не могут быть меньше или равны нулю: width={width}, height={height}, length={length}, weight={weight}");
        }

        return new Box(width, height, length, weight, productionDate, expirationDate, palletId);
    }

    public void Update(
        double? width,
        double? height,
        double? length,
        double? weight,
        DateOnly? productionDate,
        DateOnly? expirationDate,
        long? palletId)
    {
        if (!ArgumentsPositive(width, height, length, weight))
        {
            throw new ArgumentException($"Измерения коробки не могут быть меньше или равны нулю: width={width}, height={height}, length={length}, weight={weight}");
        }

        Width = width ?? Width;
        Height = height ?? Height;
        Length = length ?? Length;
        Weight = weight ?? Weight;
        ProductionDate = productionDate ?? ProductionDate;
        ExpirationDate = expirationDate ?? ExpirationDate;
        PalletId = palletId ?? PalletId;
    }

    public override string ToString()
    {
        return $"Коробка #{Id} | Ш×В×Г: {Width} × {Height} × {Length} | Объём: {Volume} | Вес: {Weight} | Срок годности: {ExpirationDate}";
    }
}

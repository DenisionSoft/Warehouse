namespace Warehouse.Domain.Entities;

public sealed class Pallet : Item
{
    private const double EmptyPalletWeight = 30;

    public List<Box> Boxes { get; private set; } = [];

    public override double Weight => Math.Round(EmptyPalletWeight + Boxes.Sum(b => b.Weight), 2);

    public override double Volume => Math.Round(base.Volume + Boxes.Sum(box => box.Volume), 2);

    public override DateOnly? ExpirationDate => Boxes.Min(b => b.ExpirationDate);

    /// <summary>
    /// Конструктор <c>Pallets</c> с параметрами ширины, высоты и глубины паллеты. Базовый вес паллеты - 30 кг.
    /// </summary>
    /// <param name="width">Ширина паллеты.</param>
    /// <param name="height">Высота паллеты.</param>
    /// <param name="length">Глубина паллеты.</param>
    private Pallet(double width, double height, double length) : base(width, height, length)
    {
    }

    public static Pallet Create(double width, double height, double length)
    {
        if (!ArgumentsPositive(width, height, length))
        {
            throw new ArgumentException($"Измерения паллеты не могут быть меньше или равны нулю: width={width}, height={height}, length={length}");
        }
        return new Pallet(width, height, length);
    }

    public void Update(double? width, double? height, double? length)
    {
        if (!ArgumentsPositive(width, height, length))
        {
            throw new ArgumentException($"Измерения паллеты не могут быть меньше или равны нулю: width={width}, height={height}, length={length}");
        }

        var unfittingBox = GetUnfittingBox(width, length);
        if (unfittingBox != null)
        {
            throw new ArgumentException(
                $"Ширина и глубина паллеты (width={width}, length={length}) не могут быть меньше ширины и глубины хранящихся коробок (неподходящая коробка id: {unfittingBox.Id}, ширина: {unfittingBox.Width}, глубина: {unfittingBox.Length})");
        }

        Width = width ?? Width;
        Height = height ?? Height;
        Length = length ?? Length;
    }

    public void SetBoxes(List<Box> boxes)
    {
        foreach (var box in boxes)
        {
            if (!IsBoxFitting(box.Width, box.Length))
            {
                throw new ArgumentException($"Ширина и глубина коробки (width={box.Width}, length={box.Length}) не может быть больше ширины и глубины паллеты (width={Width}, length={Length})");
            }
        }
        Boxes = boxes;
    }

    /// <summary>
    /// Метод <c>PrintBoxes</c> печатает каждую коробку на паллете
    /// </summary>
    public void PrintBoxes()
    {
        if (Boxes.Count == 0)
        {
            Console.WriteLine("Паллета пуста");
            return;
        }
        foreach (var box in Boxes)
        {
            Console.WriteLine(box);
        }
    }

    public override string ToString()
    {
        if (Boxes.Count == 0)
        {
            return $"Паллета #{Id} | Ш×В×Г: {Width} × {Height} × {Length} | Объём: {Volume} | Вес: {Weight} | Срок годности: --";
        }
        return $"Паллета #{Id} | Ш×В×Г: {Width} × {Height} × {Length} | Объём: {Volume} | Вес: {Weight} | Срок годности: {ExpirationDate}";
    }

    public bool IsBoxFitting(double? width, double? length)
    {
        if (width != null && width > Width)
        {
            return false;
        }
        if (length != null && length > Length)
        {
            return false;
        }
        return true;
    }

    private Box? GetUnfittingBox(double? width, double? length)
    {
        return Boxes.FirstOrDefault(box => (width != null && box.Width > width) || (length != null && box.Length > length));
    }
}

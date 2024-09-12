namespace Warehouse.Models;

public class Pallet : Item
{
    public List<Box> Boxes { get; set; } = new();

    /// <summary>
    /// Конструктор <c>Pallet</c> с параметрами ширины, высоты и глубины паллеты. Базовый вес паллеты - 30 кг.
    /// </summary>
    /// <param name="width">Ширина паллеты.</param>
    /// <param name="height">Высота паллеты.</param>
    /// <param name="length">Глубина паллеты.</param>
    public Pallet(double width, double height, double length)
    {
        Width = width;
        Height = height;
        Length = length;
        Weight = 30;
    }
    
    /// <summary>
    /// Метод <c>GetVolume</c> для паллеты, считающий объём паллеты с учётом объёма коробок
    /// </summary>
    /// <returns>
    /// double - объём паллеты с учётом объёма коробок
    /// </returns>
    public override double GetVolume() => Math.Round(base.GetVolume() + Boxes.Sum(box => box.GetVolume()), 2);
    
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
            return $"Паллета #{Id} | Ш×В×Г: {Width} × {Height} × {Length} | Объём: {GetVolume()} | Вес: {Weight} | Срок годности: --";
        }
        return $"Паллета #{Id} | Ш×В×Г: {Width} × {Height} × {Length} | Объём: {GetVolume()} | Вес: {Math.Round(Weight + Boxes.Sum(b => b.Weight), 2) } | Срок годности: {Boxes.Min(b => b.ExpirationDate)}";
    }
    
}
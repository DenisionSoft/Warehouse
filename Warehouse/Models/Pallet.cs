namespace Warehouse.Models;

public class Pallet : Item
{
    public List<Box> Boxes { get; set; } = new();

    public Pallet(double width, double height, double length)
    {
        Width = width;
        Height = height;
        Length = length;
        Weight = 30;
    }
    
    public override double GetVolume() => Math.Round(base.GetVolume() + Boxes.Sum(box => box.GetVolume()), 2);
    
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
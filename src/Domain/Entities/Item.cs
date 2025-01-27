namespace Warehouse.Domain.Entities;

public abstract class Item
{
    public long Id;
    public double Width { get; protected set; }
    public double Height { get; protected set; }
    public double Length { get; protected set; }
    public virtual double Weight { get; protected set; }
    public virtual double Volume => Math.Round(Width * Height * Length, 2);
    public virtual DateOnly? ExpirationDate { get; protected set; }

    protected Item(double width, double height, double length)
    {
        Width = width;
        Height = height;
        Length = length;
    }

    protected static bool ArgumentsPositive(params double?[] numbers)
    {
        foreach (var number in numbers)
        {
            if (number != null && number <= 0)
            {
                return false;
            }
        }
        return true;
    }
}

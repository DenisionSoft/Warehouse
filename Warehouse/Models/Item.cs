namespace Warehouse.Models;

public abstract class Item
{
    public long Id;
    public double Width { get; set; }
    public double Height { get; set; }
    public double Length { get; set; }
    public double Weight { get; set; }

    public virtual double GetVolume() => Math.Round(Width * Height * Length, 2);
}
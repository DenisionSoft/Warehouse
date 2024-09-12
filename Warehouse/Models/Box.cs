namespace Warehouse.Models;

public class Box : Item
{
    public DateOnly? ProductionDate { get; set; }
    public DateOnly ExpirationDate { get; set; }
    
    public long PalletId { get; set; }
    public Pallet Pallet { get; set; }
    
    public override string ToString()
    {
        return $"Коробка #{Id} | Ш×В×Г: {Width} × {Height} × {Length} | Объём: {GetVolume()} | Вес: {Weight} | Срок годности: {ExpirationDate}";
    }
}
using Microsoft.EntityFrameworkCore;
using Warehouse.Models;

namespace Warehouse.Repositories;

public class BoxesRepository
{
    private readonly WarehouseContext _context;
    
    public BoxesRepository(WarehouseContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Метод <c>AddAsync</c> добавляет коробку в базу данных.
    /// </summary>
    /// <param name="box">Коробка, которую нужно добавить в базу данных.</param>
    public virtual async Task AddAsync(Box box)
    {
        await _context.Boxes.AddAsync(box);
        await _context.SaveChangesAsync();
    }
    
    /// <summary>
    /// Метод <c>GetAll</c> возвращает все коробки из базы данных.
    /// </summary>
    public async Task<List<Box>> GetAll()
    {
        return await _context.Boxes.ToListAsync();
    }
    
    /// <summary>
    /// Метод <c>GetAllByPalletId</c> возвращает все коробки, которые находятся на паллете с заданным идентификатором.
    /// </summary>
    /// <param name="palletId">Идентификатор паллеты, на которой находятся коробки.</param>
    /// <returns>Список коробок, которые находятся на паллете с заданным идентификатором.</returns>
    public async Task<List<Box>> GetAllByPalletId(long palletId)
    {
        return await _context.Boxes
            .Where(b => b.PalletId == palletId)
            .ToListAsync();
    }
}
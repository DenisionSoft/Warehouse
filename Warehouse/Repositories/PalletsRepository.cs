using Microsoft.EntityFrameworkCore;
using Warehouse.Models;

namespace Warehouse.Repositories;

public class PalletsRepository
{
    private readonly WarehouseContext _context;
    
    public PalletsRepository(WarehouseContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Метод <c>AddAsync</c> добавляет паллету в базу данных.
    /// </summary>
    /// <param name="pallet">Паллета, которую нужно добавить в базу данных.</param>
    public virtual async Task AddAsync(Pallet pallet)
    {
        await _context.Pallets.AddAsync(pallet);
        await _context.SaveChangesAsync();
    }
    
    /// <summary>
    /// Метод <c>AddRangeAsync</c> добавляет паллеты из списка в базу данных.
    /// </summary>
    /// <param name="pallets">Список паллет, которые нужно добавить в базу данных.</param>
    public virtual async Task AddRangeAsync(List<Pallet> pallets)
    {
        await _context.Pallets.AddRangeAsync(pallets);
        await _context.SaveChangesAsync();
    }
    
    /// <summary>
    /// Метод <c>GetById</c> возвращает паллету по её идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор паллеты.</param>
    /// <returns>Паллета с заданным идентификатором.</returns>
    public virtual async Task<Pallet?> GetById(long id)
    {
        return await _context.Pallets.FindAsync(id);
    }
    
    /// <summary>
    /// Метод <c>GetAll</c> возвращает все паллеты из базы данных.
    /// </summary>
    /// <returns>Список всех паллет из базы данных.</returns>
    public async Task<List<Pallet>> GetAll()
    {
        return await _context.Pallets.ToListAsync();
    }

    /// <summary>
    /// Метод <c>GetAllGroupedByExpirationDateSortedByWeight</c> возвращает все паллеты, у которых есть коробки, сгруппированные по дате истечения срока годности и отсортированные по весу.
    /// </summary>
    /// <returns>Список всех паллет, у которых есть коробки, сгруппированных по дате истечения срока годности и отсортированных по весу.</returns>
    public virtual async Task<List<Pallet>> GetAllGroupedByExpirationDateSortedByWeight()
    {
        return await _context.Pallets
            .Where(p => p.Boxes.Any())
            .Include(p => p.Boxes)
            .OrderBy(p => p.Boxes.Min(b => b.ExpirationDate))
            .ThenBy(p => p.Weight + p.Boxes.Sum(b => b.Weight))
            .ToListAsync();
    }
    
    /// <summary>
    /// Метод <c>GetThreeWithLatestExpiryBoxesOrderByVolume</c> возвращает три паллеты с коробками, у которых самая поздняя дата истечения срока годности, отсортированные по объему.
    /// </summary>
    /// <returns>Список из трёх паллет с коробками, у которых самая поздняя дата истечения срока годности, отсортированных по объему.</returns>
    public async Task<List<Pallet>> GetThreeWithLatestExpiryBoxesOrderByVolume()
    {
        var pallets = await _context.Pallets
            .Where(p => p.Boxes.Any())
            .Include(p => p.Boxes)
            .OrderByDescending(p => p.Boxes.Max(b => b.ExpirationDate))
            .Take(3)
            .ToListAsync();

        return pallets
            .OrderBy(p => p.GetVolume())
            .ToList();
    }
}
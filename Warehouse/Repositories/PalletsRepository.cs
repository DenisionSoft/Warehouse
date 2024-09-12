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

    public virtual async Task AddAsync(Pallet pallet)
    {
        await _context.Pallets.AddAsync(pallet);
        await _context.SaveChangesAsync();
    }
    
    public virtual async Task AddRangeAsync(List<Pallet> pallets)
    {
        await _context.Pallets.AddRangeAsync(pallets);
        await _context.SaveChangesAsync();
    }
    
    public virtual async Task<Pallet?> GetById(long id)
    {
        return await _context.Pallets.FindAsync(id);
    }
    
    public async Task<List<Pallet>> GetAll()
    {
        return await _context.Pallets.ToListAsync();
    }

    public virtual async Task<List<Pallet>> GetAllGroupedByExpirationDateSortedByWeight()
    {
        return await _context.Pallets
            .Where(p => p.Boxes.Any())
            .Include(p => p.Boxes)
            .OrderBy(p => p.Boxes.Min(b => b.ExpirationDate))
            .ThenBy(p => p.Weight + p.Boxes.Sum(b => b.Weight))
            .ToListAsync();
    }
    
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
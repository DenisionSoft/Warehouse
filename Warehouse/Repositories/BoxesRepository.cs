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

    public virtual async Task AddAsync(Box box)
    {
        await _context.Boxes.AddAsync(box);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<Box>> GetAll()
    {
        return await _context.Boxes.ToListAsync();
    }
    
    public async Task<List<Box>> GetAllByPalletId(long palletId)
    {
        return await _context.Boxes
            .Where(b => b.PalletId == palletId)
            .ToListAsync();
    }
}
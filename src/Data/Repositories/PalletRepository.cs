using Microsoft.EntityFrameworkCore;
using Warehouse.Business.Pallets;
using Warehouse.Domain.Entities;

namespace Warehouse.Data.Repositories;

internal sealed class PalletRepository : IPalletRepository
{
    private readonly WarehouseDbContext dbContext;

    public PalletRepository(WarehouseDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <summary>
    /// Метод <c>GetAllAsync</c> возвращает все паллеты из базы данных.
    /// </summary>
    /// <returns>Список всех паллет из базы данных.</returns>
    public async Task<IReadOnlyList<Pallet>> GetAllAsync(
        int offset,
        int limit,
        CancellationToken cancellationToken)
    {
        return await dbContext.Pallets.Skip(offset).Take(limit).Include(p => p.Boxes).ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Метод <c>GetByIdAsync</c> возвращает паллету по её идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор паллеты.</param>
    /// <returns>Паллета с заданным идентификатором.</returns>
    public async Task<Pallet?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await dbContext.Pallets.Include(p => p.Boxes).FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    /// <summary>
    /// Метод <c>AddAsync</c> добавляет паллету в базу данных.
    /// </summary>
    /// <param name="entity">Паллета, которую нужно добавить в базу данных.</param>
    public async Task<Pallet> AddAsync(Pallet entity, CancellationToken cancellationToken)
    {
        var palletEntry = await dbContext.Pallets.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return palletEntry.Entity;
    }

    /// <summary>
    /// Метод <c>AddRangeAsync</c> добавляет паллеты из списка в базу данных.
    /// </summary>
    /// <param name="entities">Список паллет, которые нужно добавить в базу данных.</param>
    public async Task AddRangeAsync(IReadOnlyList<Pallet> entities, CancellationToken cancellationToken)
    {
        await dbContext.Pallets.AddRangeAsync(entities, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Pallet> UpdateAsync(Pallet entity, CancellationToken cancellationToken)
    {
        var palletEntry = dbContext.Pallets.Update(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        return palletEntry.Entity;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Pallets.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        dbContext.Pallets.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Метод <c>GetAllGroupedByExpirationDateSortedByWeightAsync</c> возвращает все паллеты, у которых есть коробки, сгруппированные по дате истечения срока годности и отсортированные по весу.
    /// </summary>
    /// <returns>Список всех паллет, у которых есть коробки, сгруппированных по дате истечения срока годности и отсортированных по весу.</returns>
    public async Task<IReadOnlyList<Pallet>> GetAllGroupedByExpirationDateSortedByWeightAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Pallets
            .Where(p => p.Boxes.Any())
            .Include(p => p.Boxes)
            .OrderBy(p => p.Boxes.Min(b => b.ExpirationDate))
            .ThenBy(p => p.Weight)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Метод <c>GetThreeWithLatestExpiryBoxesOrderByVolumeAsync</c> возвращает три паллеты с коробками, у которых самая поздняя дата истечения срока годности, отсортированные по объему.
    /// </summary>
    /// <returns>Список из трёх паллет с коробками, у которых самая поздняя дата истечения срока годности, отсортированных по объему.</returns>
    public async Task<IReadOnlyList<Pallet>> GetThreeWithLatestExpiryBoxesOrderByVolumeAsync(CancellationToken cancellationToken)
    {
        var pallets = await dbContext.Pallets
            .Where(p => p.Boxes.Any())
            .Include(p => p.Boxes)
            .OrderByDescending(p => p.Boxes.Max(b => b.ExpirationDate))
            .Take(3)
            .ToListAsync(cancellationToken);

        return pallets
            .OrderBy(p => p.Volume)
            .ToList();
    }
}

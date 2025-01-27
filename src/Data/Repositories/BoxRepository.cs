using Microsoft.EntityFrameworkCore;
using Warehouse.Business.Boxes;
using Warehouse.Domain.Entities;

namespace Warehouse.Data.Repositories;

internal sealed class BoxRepository : IBoxRepository
{
    private readonly WarehouseDbContext dbContext;

    public BoxRepository(WarehouseDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <summary>
    /// Метод <c>GetAllAsync</c> возвращает все коробки из базы данных.
    /// </summary>
    /// <returns>Список всех коробок из базы данных.</returns>
    public async Task<IReadOnlyList<Box>> GetAllAsync(
        int offset,
        int limit,
        CancellationToken cancellationToken)
    {
        return await dbContext.Boxes.Skip(offset).Take(limit).ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Метод <c>GetByIdAsync</c> возвращает коробку по её идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор коробки.</param>
    /// <returns>Коробка с заданным идентификатором.</returns>
    public async Task<Box?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await dbContext.Boxes.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    /// <summary>
    /// Метод <c>AddAsync</c> добавляет коробку в базу данных.
    /// </summary>
    /// <param name="entity">Коробка, которую нужно добавить в базу данных.</param>
    public async Task<Box> AddAsync(Box entity, CancellationToken cancellationToken)
    {
        var boxEntry = await dbContext.Boxes.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return boxEntry.Entity;
    }

    /// <summary>
    /// Метод <c>AddRangeAsync</c> добавляет коробки из списка в базу данных.
    /// </summary>
    /// <param name="entities">Список коробок, которые нужно добавить в базу данных.</param>
    public async Task AddRangeAsync(IReadOnlyList<Box> entities, CancellationToken cancellationToken)
    {
        await dbContext.Boxes.AddRangeAsync(entities, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Box> UpdateAsync(Box entity, CancellationToken cancellationToken)
    {
        var boxEntry = dbContext.Boxes.Update(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        return boxEntry.Entity;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Boxes.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        dbContext.Boxes.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Метод <c>GetAllByPalletIdAsync</c> возвращает все коробки, которые находятся на паллете с заданным идентификатором.
    /// </summary>
    /// <param name="palletId">Идентификатор паллеты, на которой находятся коробки.</param>
    /// <returns>Список коробок, которые находятся на паллете с заданным идентификатором.</returns>
    public async Task<IReadOnlyList<Box>> GetAllByPalletIdAsync(long palletId, CancellationToken cancellationToken)
    {
        return await dbContext.Boxes
            .Where(b => b.PalletId == palletId)
            .ToListAsync(cancellationToken);
    }
}

namespace Warehouse.Business;

public interface IRepository<TEntity>
{
    Task<IReadOnlyList<TEntity>> GetAllAsync(int offset = 0, int limit = 10, CancellationToken cancellationToken = default);

    Task<TEntity?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task AddRangeAsync(IReadOnlyList<TEntity> entities, CancellationToken cancellationToken = default);

    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}

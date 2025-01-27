using Warehouse.Domain.Entities;

namespace Warehouse.Business.Boxes;

public interface IBoxService
{
    public Task<IReadOnlyList<Box>> GetAllAsync(int offset = 0, int limit = 10, CancellationToken cancellationToken = default);

    public Task<Box> GetByIdAsync(long boxId, CancellationToken cancellationToken = default);

    public Task<Box> AddAsync(long palletId, CreateBoxCommand boxCommand, CancellationToken cancellationToken = default);

    public Task<Box> UpdateAsync(long palletId, long boxId, UpdateBoxCommand boxCommand, CancellationToken cancellationToken = default);

    public Task DeleteAsync(long boxId, CancellationToken cancellationToken = default);

    public Task<IReadOnlyList<Box>> GetAllByPalletIdAsync(long palletId, CancellationToken cancellationToken = default);
}

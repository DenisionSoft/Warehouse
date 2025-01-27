using Warehouse.Domain.Entities;

namespace Warehouse.Business.Pallets;

public interface IPalletService
{
    public Task<IReadOnlyList<Pallet>> GetAllAsync(int offset = 0, int limit = 10, CancellationToken cancellationToken = default);

    public Task<Pallet> GetByIdAsync(long palletId, CancellationToken cancellationToken = default);

    public Task<Pallet> AddAsync(CreatePalletCommand palletCommand, CancellationToken cancellationToken = default);

    public Task AddRangeAsync(IReadOnlyList<Pallet> pallets, CancellationToken cancellationToken = default);

    public Task<Pallet> UpdateAsync(long palletId, UpdatePalletCommand palletCommand, CancellationToken cancellationToken = default);

    public Task DeleteAsync(long palletId, CancellationToken cancellationToken = default);

    public Task<IReadOnlyList<Pallet>> GetMostExpiredAsync(CancellationToken cancellationToken = default);

    public Task<IReadOnlyList<Pallet>> GetLeastExpiredByBoxesAsync(CancellationToken cancellationToken = default);
}

using Warehouse.Business.Exceptions;
using Warehouse.Domain.Entities;

namespace Warehouse.Business.Pallets;

public sealed class PalletService : IPalletService
{
    private readonly IPalletRepository palletRepository;

    public PalletService(IPalletRepository palletRepository)
    {
        this.palletRepository = palletRepository;
    }

    public Task<IReadOnlyList<Pallet>> GetAllAsync(int offset, int limit, CancellationToken cancellationToken)
    {
        return palletRepository.GetAllAsync(offset, limit, cancellationToken);
    }

    public Task<Pallet> GetByIdAsync(long palletId, CancellationToken cancellationToken)
    {
        return GetRequiredPallet(palletId, cancellationToken);
    }

    public async Task<Pallet> AddAsync(CreatePalletCommand palletCommand, CancellationToken cancellationToken)
    {
        var pallet = Pallet.Create(palletCommand.Width, palletCommand.Height, palletCommand.Length);
        return await palletRepository.AddAsync(pallet, cancellationToken);
    }

    public Task AddRangeAsync(IReadOnlyList<Pallet> pallets, CancellationToken cancellationToken)
    {
        return palletRepository.AddRangeAsync(pallets,  cancellationToken);
    }

    public async Task<Pallet> UpdateAsync(long palletId, UpdatePalletCommand palletCommand, CancellationToken cancellationToken)
    {
        var pallet = await GetRequiredPallet(palletId, cancellationToken);
        pallet.Update(palletCommand.Width, palletCommand.Height, palletCommand.Length);
        return await palletRepository.UpdateAsync(pallet, cancellationToken);
    }

    public Task DeleteAsync(long palletId, CancellationToken cancellationToken = default)
    {
        return palletRepository.DeleteAsync(palletId, cancellationToken);
    }

    /// <summary>
    /// Метод <c>GetMostExpiredAsync</c> возвращается все паллеты, у которых есть коробки, сгруппированные по дате истечения срока годности и отсортированные по весу.
    /// </summary>
    /// <returns>Список всех паллет, у которых есть коробки, сгруппированных по дате истечения срока годности и отсортированных по весу.</returns>
    public Task<IReadOnlyList<Pallet>> GetMostExpiredAsync(CancellationToken cancellationToken)
    {
        return palletRepository.GetAllGroupedByExpirationDateSortedByWeightAsync(cancellationToken);
    }

    /// <summary>
    /// Метод <c>GetLeastExpiredByBoxesAsync</c> возвращает три паллеты с коробками, у которых срок годности истекает позже всех остальных, отсортированные по объему.
    /// </summary>
    /// <returns>Список трех паллет с коробками, у которых срок годности истекает позже всех остальных, отсортированных по объему.</returns>
    public Task<IReadOnlyList<Pallet>> GetLeastExpiredByBoxesAsync(CancellationToken cancellationToken)
    {
        return palletRepository.GetThreeWithLatestExpiryBoxesOrderByVolumeAsync(cancellationToken);
    }

    private async Task<Pallet> GetRequiredPallet(long palletId, CancellationToken cancellationToken)
    {
        return await palletRepository.GetByIdAsync(palletId, cancellationToken)
               ?? throw new EntityNotFoundException("Паллеты с указанным идентификатором не существует", palletId.ToString());
    }
}

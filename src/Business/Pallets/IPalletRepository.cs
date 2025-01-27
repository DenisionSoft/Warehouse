using Warehouse.Domain.Entities;

namespace Warehouse.Business.Pallets;

public interface IPalletRepository : IRepository<Pallet>
{
    /// <summary>
    /// Метод <c>GetAllGroupedByExpirationDateSortedByWeightAsync</c> возвращает все паллеты, сгруппированные по дате их окончания годности
    /// и отсортированные по весу.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>Список всех паллет, сгруппированных по дате их окончания годности и отсортированных по весу.</returns>
    Task<IReadOnlyList<Pallet>> GetAllGroupedByExpirationDateSortedByWeightAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Метод <c>GetThreeWithLatestExpiryBoxesOrderByVolumeAsync</c> возвращает три паллеты с самыми поздними датами истечения срока годности
    /// среди коробок, отсортированные по объему.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>Список из трёх паллет с самыми поздними датами истечения срока годности среди коробок, отсортированных по объему.</returns>
    Task<IReadOnlyList<Pallet>> GetThreeWithLatestExpiryBoxesOrderByVolumeAsync(CancellationToken cancellationToken);
}

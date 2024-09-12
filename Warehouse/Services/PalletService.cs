using Warehouse.Models;
using Warehouse.Repositories;
using Warehouse.Utils;

namespace Warehouse.Services;

public class PalletService
{
    private readonly PalletsRepository _palletsRepository;
    
    public PalletService(PalletsRepository palletsRepository)
    {
        _palletsRepository = palletsRepository;
    }
    
    /// <summary>
    /// Метод <c>AddAsync</c> добавляет паллету в базу данных.
    /// </summary>
    /// <param name="palletDto">DTO паллеты, которую нужно добавить в базу данных.</param>
    public async Task AddAsync(NewPalletDto palletDto)
    {
        var pallet = new Pallet(palletDto.Width, palletDto.Height, palletDto.Length);
        await _palletsRepository.AddAsync(pallet);
    }
    
    /// <summary>
    /// Метод <c>AddRangeAsync</c> добавляет паллеты из списка в базу данных.
    /// </summary>
    /// <param name="pallets">Список паллет, которые нужно добавить в базу данных.</param>
    public async Task AddRangeAsync(List<Pallet> pallets)
    {
        await _palletsRepository.AddRangeAsync(pallets);
    }
    
    /// <summary>
    /// Метод <c>GetMostExpired</c> возвращается все паллеты, у которых есть коробки, сгруппированные по дате истечения срока годности и отсортированные по весу.
    /// </summary>
    /// <returns>Список всех паллет, у которых есть коробки, сгруппированных по дате истечения срока годности и отсортированных по весу.</returns>
    public async Task<List<Pallet>> GetMostExpired()
    {
        return await _palletsRepository.GetAllGroupedByExpirationDateSortedByWeight();
    }

    /// <summary>
    /// Метод <c>GetLeastExpiredByBoxes</c> возвращает три паллеты с коробками, у которых срок годности истекает позже всех остальных, отсортированные по объему.
    /// </summary>
    /// <returns>Список трех паллет с коробками, у которых срок годности истекает позже всех остальных, отсортированных по объему.</returns>
    public async Task<List<Pallet>> GetLeastExpiredByBoxes()
    {
        return await _palletsRepository.GetThreeWithLatestExpiryBoxesOrderByVolume();
    }

}
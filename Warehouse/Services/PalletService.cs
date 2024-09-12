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
    
    public async Task AddAsync(NewPalletDto palletDto)
    {
        var pallet = new Pallet(palletDto.Width, palletDto.Height, palletDto.Length);
        await _palletsRepository.AddAsync(pallet);
    }
    
    public async Task AddRangeAsync(List<Pallet> pallets)
    {
        await _palletsRepository.AddRangeAsync(pallets);
    }
    
    public async Task<List<Pallet>> GetMostExpired()
    {
        return await _palletsRepository.GetAllGroupedByExpirationDateSortedByWeight();
    }

    public async Task<List<Pallet>> GetLeastExpiredByBoxes()
    {
        return await _palletsRepository.GetThreeWithLatestExpiryBoxesOrderByVolume();
    }

}
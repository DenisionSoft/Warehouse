using Warehouse.Models;
using Warehouse.Repositories;
using Warehouse.Utils;

namespace Warehouse.Services;

public class BoxService
{
    private readonly BoxesRepository _boxesRepository;
    private readonly PalletsRepository _palletsRepository;
    
    public BoxService(BoxesRepository boxesRepository, PalletsRepository palletsRepository)
    {
        _boxesRepository = boxesRepository;
        _palletsRepository = palletsRepository;
    }
    
    public async Task AddAsync(NewBoxDto boxDto)
    {
        var (width, height, length, weight, givenDate, isExpirationDate, palletId) = boxDto;
        
        Pallet? pallet = await _palletsRepository.GetById(palletId);
        
        if (pallet == null)
            throw new ArgumentException("паллеты с указанным идентификатором не существует.");
        
        if (width > pallet.Width || length > pallet.Length)
            throw new ArgumentException("коробка слишком велика для этой паллеты.");
        
        if (isExpirationDate)
            await _boxesRepository.AddAsync(new Box
            {
                Width = width,
                Height = height,
                Length = length,
                Weight = weight,
                ProductionDate = null,
                ExpirationDate = givenDate,
                PalletId = palletId
            });
        else
            await _boxesRepository.AddAsync(new Box
            {
                Width = width,
                Height = height,
                Length = length,
                Weight = weight,
                ProductionDate = givenDate,
                ExpirationDate = givenDate.AddDays(100),
                PalletId = palletId
            });
    }
    
    public async Task<List<Box>> GetAllByPalletId(long palletId)
    {
        return await _boxesRepository.GetAllByPalletId(palletId);
    }
}
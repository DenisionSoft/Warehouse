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
    
    /// <summary>
    /// Метод <c>AddAsync</c> добавляет коробку в базу данных. Он проверяет, существует ли паллета с указанным идентификатором и соответствует ли размер коробки размерам паллеты.
    /// </summary>
    /// <param name="boxDto">DTO коробки, которую нужно добавить в базу данных.</param>
    /// <exception cref="ArgumentException">Паллеты с указанным идентификатором не существует.</exception>
    /// <exception cref="ArgumentException">Коробка слишком велика для этой паллеты.</exception>
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
    
    /// <summary>
    /// Метод <c>GetAllByPalletId</c> возвращает все коробки, которые находятся на паллете с заданным идентификатором.
    /// </summary>
    /// <param name="palletId">Идентификатор паллеты, на которой находятся коробки.</param>
    /// <returns>Список коробок, которые находятся на паллете с заданным идентификатором.</returns>
    public async Task<List<Box>> GetAllByPalletId(long palletId)
    {
        return await _boxesRepository.GetAllByPalletId(palletId);
    }
}
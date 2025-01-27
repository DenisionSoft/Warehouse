using Warehouse.Business.Exceptions;
using Warehouse.Business.Pallets;
using Warehouse.Domain.Entities;

namespace Warehouse.Business.Boxes;

public sealed class BoxService : IBoxService
{
    private readonly IBoxRepository boxRepository;
    private readonly IPalletRepository palletRepository;

    public BoxService(IBoxRepository boxRepository, IPalletRepository palletRepository)
    {
        this.boxRepository = boxRepository;
        this.palletRepository = palletRepository;
    }

    public Task<IReadOnlyList<Box>> GetAllAsync(int offset, int limit, CancellationToken cancellationToken)
    {
        return boxRepository.GetAllAsync(offset, limit, cancellationToken);
    }

    public Task<Box> GetByIdAsync(long boxId, CancellationToken cancellationToken)
    {
        return GetRequiredBox(boxId, cancellationToken);
    }

    /// <summary>
    /// Метод <c>AddAsync</c> добавляет коробку в базу данных. Он проверяет, существует ли паллета с указанным идентификатором и соответствует ли размер коробки размерам паллеты.
    /// </summary>
    /// <param name="boxCommand">DTO коробки, которую нужно добавить в базу данных.</param>
    /// <exception cref="ArgumentException">Паллеты с указанным идентификатором не существует.</exception>
    /// <exception cref="ArgumentException">Коробка слишком велика для этой паллеты.</exception>
    public async Task<Box> AddAsync(long palletId, CreateBoxCommand boxCommand, CancellationToken cancellationToken)
    {
        var (width, height, length, weight, givenDate, isExpirationDate) = boxCommand;

        var pallet = await GetRequiredPallet(palletId, cancellationToken);

        if(!pallet.IsBoxFitting(width, length))
        {
            throw new ArgumentException($"Ширина и глубина коробки (width={width}, length={length}) не может быть больше ширины и глубины паллеты (width={pallet.Width}, length={pallet.Length})");
        }

        DateOnly? productionDate = null;
        DateOnly expirationDate;

        if (isExpirationDate)
        {
            expirationDate = givenDate;
        }
        else
        {
            productionDate = givenDate;
            expirationDate = givenDate.AddDays(100);
        }

        var box = Box.Create(width, height, length, weight, productionDate, expirationDate, palletId);

        return await boxRepository.AddAsync(box, cancellationToken);
    }

    public async Task<Box> UpdateAsync(long palletId, long boxId, UpdateBoxCommand boxCommand, CancellationToken cancellationToken = default)
    {
        var pallet = await GetRequiredPallet(palletId, cancellationToken);
        var box = await GetRequiredBox(boxId, cancellationToken);

        if (box.PalletId != pallet.Id)
        {
            throw new ArgumentException($"Коробка с id={boxId} не принадлежит паллете с id={palletId}");
        }

        if (boxCommand.PalletId != null && boxCommand.PalletId != palletId)
        {
            pallet = await GetRequiredPallet(boxCommand.PalletId.Value, cancellationToken);
        }

        if (!pallet.IsBoxFitting(boxCommand.Width, boxCommand.Length))
        {
            throw new ArgumentException($"Ширина и глубина коробки (width={boxCommand.Width}, length={boxCommand.Length}) не может быть больше ширины и глубины паллеты (width={pallet.Width}, length={pallet.Length})");
        }

        box.Update(
            boxCommand.Width,
            boxCommand.Height,
            boxCommand.Length,
            boxCommand.Weight,
            boxCommand.ProductionDate,
            boxCommand.ExpirationDate,
            boxCommand.PalletId
        );

        return await boxRepository.UpdateAsync(box, cancellationToken);
    }

    public Task DeleteAsync(long boxId, CancellationToken cancellationToken)
    {
        return boxRepository.DeleteAsync(boxId, cancellationToken);
    }

    /// <summary>
    /// Метод <c>GetAllByPalletIdAsync</c> возвращает все коробки, которые находятся на паллете с заданным идентификатором.
    /// </summary>
    /// <param name="palletId">Идентификатор паллеты, на которой находятся коробки.</param>
    /// <returns>Список коробок, которые находятся на паллете с заданным идентификатором.</returns>
    public async Task<IReadOnlyList<Box>> GetAllByPalletIdAsync(long palletId, CancellationToken cancellationToken)
    {
        await GetRequiredPallet(palletId, cancellationToken);
        return await boxRepository.GetAllByPalletIdAsync(palletId, cancellationToken);
    }

    private async Task<Pallet> GetRequiredPallet(long palletId, CancellationToken cancellationToken)
    {
        return await palletRepository.GetByIdAsync(palletId, cancellationToken)
            ?? throw new EntityNotFoundException("Паллеты с указанным идентификатором не существует", palletId.ToString());
    }

    private async Task<Box> GetRequiredBox(long boxId, CancellationToken cancellationToken)
    {
        return await boxRepository.GetByIdAsync(boxId, cancellationToken)
            ?? throw new EntityNotFoundException("Коробки с указанным идентификатором не существует", boxId.ToString());
    }
}

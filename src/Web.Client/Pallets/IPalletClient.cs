using Warehouse.Web.Contracts.Models;
using Warehouse.Web.Contracts.Models.Pallet;

namespace Warehouse.Web.Client.Pallets;

public interface IPalletClient
{
    Task<GetAllResponse<PalletResponse>> GetAllPallets(PaginationParams paginationParams, CancellationToken cancellationToken = default);

    Task<PalletResponse?> GetPalletById(long palletId, CancellationToken cancellationToken = default);

    Task<PalletResponse?> AddPallet(CreatePalletRequest request, CancellationToken cancellationToken = default);

    Task<PalletResponse?> UpdatePallet(long palletId, UpdatePalletRequest request, CancellationToken cancellationToken = default);

    Task DeletePallet(long palletId, CancellationToken cancellationToken = default);
}

using Warehouse.Web.Contracts.Models;
using Warehouse.Web.Contracts.Models.Box;

namespace Warehouse.Web.Client.Boxes;

public interface IBoxClient
{
    Task<GetAllResponse<BoxResponse>> GetAllBoxes(PaginationParams paginationParams, CancellationToken cancellationToken = default);

    Task<GetAllResponse<BoxResponse>> GetAllBoxesByPalletId(long palletId, CancellationToken cancellationToken = default);

    Task<BoxResponse?> GetBoxById(long boxId, CancellationToken cancellationToken = default);

    Task<BoxResponse?> AddBox(long palletId, CreateBoxRequest request, CancellationToken cancellationToken = default);

    Task<BoxResponse?> UpdateBox(long palletId, long boxId, UpdateBoxRequest request, CancellationToken cancellationToken = default);

    Task DeleteBox(long boxId, CancellationToken cancellationToken = default);
}

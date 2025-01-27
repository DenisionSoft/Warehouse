using Warehouse.Domain.Entities;

namespace Warehouse.Business.Boxes;

public interface IBoxRepository : IRepository<Box>
{
    Task<IReadOnlyList<Box>> GetAllByPalletIdAsync(long palletId, CancellationToken cancellationToken);
}

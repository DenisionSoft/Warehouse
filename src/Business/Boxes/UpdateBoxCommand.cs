namespace Warehouse.Business.Boxes;

public sealed record UpdateBoxCommand
(
    double? Width,
    double? Height,
    double? Length,
    double? Weight,
    DateOnly? ProductionDate,
    DateOnly? ExpirationDate,
    long? PalletId
);

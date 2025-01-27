namespace Warehouse.Business.Pallets;

public sealed record UpdatePalletCommand
(
    double? Width,
    double? Height,
    double? Length
);

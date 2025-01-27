namespace Warehouse.Business.Pallets;

public sealed record CreatePalletCommand
(
    double Width,
    double Height,
    double Length
);

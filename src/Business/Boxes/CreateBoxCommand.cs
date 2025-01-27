namespace Warehouse.Business.Boxes;

public sealed record CreateBoxCommand
(
    double Width,
    double Height,
    double Length,
    double Weight,
    DateOnly GivenDate,
    bool IsExpirationDate
);

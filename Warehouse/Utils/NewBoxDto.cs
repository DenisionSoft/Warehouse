namespace Warehouse.Utils;

public record NewBoxDto(
    double Width,
    double Height,
    double Length,
    double Weight,
    DateOnly GivenDate,
    bool IsExpirationDate,
    long PalletId
    );
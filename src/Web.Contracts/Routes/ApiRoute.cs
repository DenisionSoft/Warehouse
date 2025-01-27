using Warehouse.Web.Contracts.Models;

namespace Warehouse.Web.Contracts.Routes;

public static class ApiRoute
{
    public const string Root = "api/v1";

    public static class Warehouse
    {
        public const string Pallets = Root + "/pallets";
        public const string Pallet = Root + "/pallets/{palletId}";
        public const string PalletBoxes = Root + "/pallets/{palletId}/boxes";
        public const string PalletBox = Root + "/pallets/{palletId}/boxes/{boxId}";

        public static string ForPallet(long palletId) => ReplaceUrlSegment(Pallet, "palletId", palletId.ToString());
        public static string ForPalletBoxes(long palletId) => ReplaceUrlSegment(PalletBoxes, "palletId", palletId.ToString());
        public static string ForPalletBox(long palletId, long boxId) => ReplaceUrlSegments(PalletBox, ("palletId", palletId.ToString()), ("boxId", boxId.ToString()));

        public const string Boxes = Root + "/boxes";
        public const string Box = Root + "/boxes/{boxId}";

        public static string ForBox(long boxId) => ReplaceUrlSegment(Box, "boxId", boxId.ToString());
    }

    public static string WithPagination(string urlTemplate, PaginationParams paginationParams)
    {
        return $"{urlTemplate}?offset={paginationParams.Offset}&limit={paginationParams.Limit}";
    }

    private static string ReplaceUrlSegment(string urlTemplate, string name, string value)
    {
        var escapedValue = Uri.EscapeDataString(value);
        return urlTemplate.Replace("{" + name + "}", escapedValue);
    }

    private static string ReplaceUrlSegments(string urlTemplate, params (string Name, string Value)[] urlSegments)
    {
        return urlSegments.Aggregate(urlTemplate, (current, urlSegment) => ReplaceUrlSegment(current, urlSegment.Name, urlSegment.Value));
    }
}

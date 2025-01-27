using System.Net.Http.Json;
using Warehouse.Web.Contracts.Models;
using Warehouse.Web.Contracts.Models.Box;
using Warehouse.Web.Contracts.Routes;

namespace Warehouse.Web.Client.Boxes;

internal sealed class BoxClient : IBoxClient
{
    private readonly HttpClient client;
    private readonly Uri baseUri;

    public BoxClient(HttpClient client)
    {
        this.client = client;
        baseUri = client.BaseAddress;
    }

    public Task<GetAllResponse<BoxResponse>> GetAllBoxes(PaginationParams paginationParams, CancellationToken cancellationToken = default)
    {
        var uri = new Uri(baseUri, ApiRoute.WithPagination(ApiRoute.Warehouse.Boxes, paginationParams));
        return client.GetFromJsonAsync<GetAllResponse<BoxResponse>>(uri, cancellationToken)!;
    }

    public Task<GetAllResponse<BoxResponse>> GetAllBoxesByPalletId(long palletId, CancellationToken cancellationToken = default)
    {
        var uri = new Uri(baseUri, ApiRoute.Warehouse.ForPalletBoxes(palletId));
        return client.GetFromJsonAsync<GetAllResponse<BoxResponse>>(uri, cancellationToken)!;
    }

    public Task<BoxResponse?> GetBoxById(long boxId, CancellationToken cancellationToken = default)
    {
        var uri = new Uri(baseUri, ApiRoute.Warehouse.ForBox(boxId));
        return client.GetFromJsonAsync<BoxResponse?>(uri, cancellationToken);
    }

    public Task<BoxResponse?> AddBox(
        long palletId,
        CreateBoxRequest request,
        CancellationToken cancellationToken = default)
    {
        var uri = new Uri(baseUri, ApiRoute.Warehouse.ForPalletBoxes(palletId));
        return client.PostAsJsonAsync<CreateBoxRequest, BoxResponse>(uri, request, cancellationToken);
    }

    public Task<BoxResponse?> UpdateBox(
        long palletId,
        long boxId,
        UpdateBoxRequest request,
        CancellationToken cancellationToken = default)
    {
        var uri = new Uri(baseUri, ApiRoute.Warehouse.ForPalletBox(palletId, boxId));
        return client.PatchAsJsonAsync<UpdateBoxRequest, BoxResponse>(uri, request, cancellationToken);
    }

    public Task DeleteBox(long boxId, CancellationToken cancellationToken = default)
    {
        var uri = new Uri(baseUri, ApiRoute.Warehouse.ForBox(boxId));
        return client.DeleteAsync(uri, cancellationToken);
    }
}

using System.Net.Http.Json;
using Warehouse.Web.Contracts.Models;
using Warehouse.Web.Contracts.Models.Pallet;
using Warehouse.Web.Contracts.Routes;

namespace Warehouse.Web.Client.Pallets;

internal sealed class PalletClient : IPalletClient
{
    private readonly HttpClient client;
    private readonly Uri baseUri;

    public PalletClient(HttpClient client)
    {
        this.client = client;
        baseUri = client.BaseAddress;
    }

    public Task<GetAllResponse<PalletResponse>> GetAllPallets(PaginationParams paginationParams, CancellationToken cancellationToken = default)
    {
        var uri = new Uri(baseUri, ApiRoute.WithPagination(ApiRoute.Warehouse.Pallets, paginationParams));
        return client.GetFromJsonAsync<GetAllResponse<PalletResponse>>(uri, cancellationToken)!;
    }

    public Task<PalletResponse?> GetPalletById(long palletId, CancellationToken cancellationToken = default)
    {
        var uri = new Uri(baseUri, ApiRoute.Warehouse.ForPallet(palletId));
        return client.GetFromJsonAsync<PalletResponse?>(uri, cancellationToken);
    }

    public Task<PalletResponse?> AddPallet(CreatePalletRequest request, CancellationToken cancellationToken = default)
    {
        var uri = new Uri(baseUri, ApiRoute.Warehouse.Pallets);
        return client.PostAsJsonAsync<CreatePalletRequest, PalletResponse>(uri, request, cancellationToken);
    }

    public Task<PalletResponse?> UpdatePallet(
        long palletId,
        UpdatePalletRequest request,
        CancellationToken cancellationToken = default)
    {
        var uri = new Uri(baseUri, ApiRoute.Warehouse.ForPallet(palletId));
        return client.PatchAsJsonAsync<UpdatePalletRequest, PalletResponse>(uri, request, cancellationToken);
    }

    public Task DeletePallet(long palletId, CancellationToken cancellationToken = default)
    {
        var uri = new Uri(baseUri, ApiRoute.Warehouse.ForPallet(palletId));
        return client.DeleteAsync(uri, cancellationToken);
    }
}

using System.Net.Http.Json;
using EShop.Shared.Models;

namespace EShop.Web.Services;

public sealed class CartApiClient(HttpClient httpClient)
{
    public async Task<ShoppingCart?> GetCartAsync(string cartId)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/carts");
        request.Headers.Add("X-Cart-Id", cartId);

        using HttpResponseMessage response = await httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<ShoppingCart>();
    }

    public async Task<ShoppingCart?> AddItemAsync(string cartId, Guid productId, int quantity)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/carts/items");
        request.Headers.Add("X-Cart-Id", cartId);
        request.Content = JsonContent.Create(new AddCartItemRequest
        {
            ProductId = productId,
            Quantity = quantity
        });

        using var response = await httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<ShoppingCart>();
    }

    public async Task<bool> RemoveItemAsync(string cartId, Guid productId)
    {
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/carts/items/{productId}");
        request.Headers.Add("X-Cart-Id", cartId);

        using var response = await httpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    public async Task<Order?> CheckoutAsync(string cartId, CheckoutRequest request)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/api/carts/checkout");
        httpRequest.Headers.Add("X-Cart-Id", cartId);
        httpRequest.Content = JsonContent.Create(request);

        using var response = await httpClient.SendAsync(httpRequest);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<Order>();
    }
}

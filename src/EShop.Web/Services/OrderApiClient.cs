using EShop.Shared.Models;

namespace EShop.Web.Services;

public sealed class OrderApiClient(HttpClient httpClient)
{
    public async Task<Order?> GetOrderAsync(Guid id) =>
        await httpClient.GetFromJsonAsync<Order>($"/orders/{id}");
}

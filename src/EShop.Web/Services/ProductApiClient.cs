using EShop.Shared.Models;

namespace EShop.Web.Services;

public sealed class ProductApiClient(HttpClient httpClient)
{
    public async Task<IReadOnlyList<Product>> GetProductsAsync()
    {
        HttpResponseMessage response = await httpClient.GetAsync("/products");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<Product>>() ?? new List<Product>();
    }
}

using System.Net.Http.Json;
using EShop.Shared.Models;

namespace EShop.Web.Services;

public sealed class CustomerApiClient(HttpClient httpClient)
{
    public async Task<IReadOnlyList<Customer>> GetAllAsync()
    {
        HttpResponseMessage response = await httpClient.GetAsync("/api/customers");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<Customer>>() ?? [];
    }

    public async Task<Customer?> GetByIdAsync(Guid id)
    {
        HttpResponseMessage response = await httpClient.GetAsync($"/api/customers/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Customer>();
    }

    public async Task<Customer> CreateAsync(Customer customer)
    {
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("/api/customers", customer);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Customer>() ?? customer;
    }

    public async Task<Customer?> UpdateAsync(Guid id, Customer customer)
    {
        HttpResponseMessage response = await httpClient.PutAsJsonAsync($"/api/customers/{id}", customer);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Customer>();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        HttpResponseMessage response = await httpClient.DeleteAsync($"/api/customers/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }

        response.EnsureSuccessStatusCode();
        return true;
    }
}

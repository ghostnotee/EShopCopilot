using EShop.Shared.Models;

namespace EShop.ApiService.Services;

public sealed class CustomerService
{
    private readonly List<Customer> _items = [];

    public IReadOnlyList<Customer> GetAll() => _items.AsReadOnly();

    public Customer? GetById(Guid id) => _items.FirstOrDefault(customer => customer.Id == id);

    public Customer Create(Customer customer)
    {
        customer.Id = customer.Id == Guid.Empty ? Guid.NewGuid() : customer.Id;
        _items.Add(customer);
        return customer;
    }

    public Customer? Update(Guid id, Customer customer)
    {
        var index = _items.FindIndex(existingCustomer => existingCustomer.Id == id);
        if (index < 0)
        {
            return null;
        }

        customer.Id = id;
        _items[index] = customer;
        return customer;
    }

    public bool Delete(Guid id)
    {
        var customer = GetById(id);
        return customer is not null && _items.Remove(customer);
    }
}

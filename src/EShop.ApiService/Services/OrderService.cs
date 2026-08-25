using EShop.ApiService.Models;

namespace EShop.ApiService.Services;

public sealed class OrderService
{
    private readonly List<Order> orders =
    [
        new()
        {
            Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            CustomerName = "Maya Chen",
            ShippingAddress = "123 Market Street, Portland, OR 97205",
            TotalAmount = 129.99m,
            Status = "Processing",
            OrderedAt = new DateTimeOffset(2026, 8, 20, 14, 30, 0, TimeSpan.Zero)
        },
        new()
        {
            Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            CustomerName = "Liam Patel",
            ShippingAddress = "456 Oak Avenue, Austin, TX 78701",
            TotalAmount = 84.50m,
            Status = "Shipped",
            OrderedAt = new DateTimeOffset(2026, 8, 22, 9, 15, 0, TimeSpan.Zero)
        }
    ];

    public IReadOnlyList<Order> GetAll() => orders.AsReadOnly();

    public Order? GetById(Guid id) => orders.FirstOrDefault(order => order.Id == id);

    public Order Create(Order order)
    {
        order.Id = order.Id == Guid.Empty ? Guid.NewGuid() : order.Id;
        orders.Add(order);
        return order;
    }

    public bool Update(Guid id, Order order)
    {
        var index = orders.FindIndex(existingOrder => existingOrder.Id == id);
        if (index < 0)
        {
            return false;
        }

        order.Id = id;
        orders[index] = order;
        return true;
    }

    public bool Delete(Guid id)
    {
        var order = GetById(id);
        return order is not null && orders.Remove(order);
    }
}

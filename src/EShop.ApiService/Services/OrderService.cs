using EShop.Shared.Models;

namespace EShop.ApiService.Services;

public sealed class OrderService
{
    private readonly List<Order> _items =
    [
        new()
        {
            Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            CustomerName = "Maya Chen",
            ShippingAddress = "123 Market Street, Portland, OR 97205",
            TotalAmount = 129.99m,
            Status = OrderStatus.Processing,
            OrderedAt = new DateTimeOffset(2026, 8, 20, 14, 30, 0, TimeSpan.Zero)
        },
        new()
        {
            Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            CustomerName = "Liam Patel",
            ShippingAddress = "456 Oak Avenue, Austin, TX 78701",
            TotalAmount = 84.50m,
            Status = OrderStatus.Shipped,
            OrderedAt = new DateTimeOffset(2026, 8, 22, 9, 15, 0, TimeSpan.Zero)
        }
    ];

    public IReadOnlyList<Order> GetAll() => _items.AsReadOnly();

    public Order? GetById(Guid id) => _items.FirstOrDefault(order => order.Id == id);

    public Order Create(Order order)
    {
        order.Id = order.Id == Guid.Empty ? Guid.NewGuid() : order.Id;
        _items.Add(order);
        return order;
    }

    /// <summary>
    /// Validates checkout details and creates an order from a shopping cart's contents.
    /// Order-shape and required-field concerns belong here rather than in CartService,
    /// which should only be responsible for cart-specific invariants.
    /// </summary>
    public Order CreateFromCart(ShoppingCart cart, CheckoutRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.CustomerName))
        {
            throw new ArgumentException("Customer name is required.", nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.ShippingAddress))
        {
            throw new ArgumentException("Shipping address is required.", nameof(request));
        }

        Order order = new()
        {
            CustomerName = request.CustomerName.Trim(),
            ShippingAddress = request.ShippingAddress.Trim(),
            TotalAmount = cart.Total,
            Status = OrderStatus.Processing,
            OrderedAt = DateTimeOffset.UtcNow
        };

        return Create(order);
    }

    public Order? Update(Guid id, Order order)
    {
        var index = _items.FindIndex(existingOrder => existingOrder.Id == id);
        if (index < 0)
        {
            return null;
        }

        order.Id = id;
        _items[index] = order;
        return order;
    }

    public bool Delete(Guid id)
    {
        var order = GetById(id);
        return order is not null && _items.Remove(order);
    }
}

using System.Collections.Concurrent;
using EShop.Shared.Models;

namespace EShop.ApiService.Services;

public sealed class CartService(ProductService productService, OrderService orderService)
{
    private readonly ConcurrentDictionary<string, ShoppingCart> carts = new();

    public ShoppingCart GetCart(string cartId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cartId);
        return carts.GetOrAdd(cartId, _ => new ShoppingCart());
    }

    public ShoppingCart AddItem(string cartId, Guid productId, int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        }

        ShoppingCart cart = GetCart(cartId);
        Product? product = productService.GetById(productId)
            ?? throw new KeyNotFoundException($"Product '{productId}' was not found.");

        CartItem? existingItem = cart.Items.FirstOrDefault(item => item.ProductId == productId);
        if (existingItem is null)
        {
            cart.Items.Add(new CartItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                UnitPrice = product.Price,
                Quantity = quantity
            });
        }
        else
        {
            existingItem.Quantity += quantity;
        }

        return cart;
    }

    public bool RemoveItem(string cartId, Guid productId)
    {
        ShoppingCart cart = GetCart(cartId);
        CartItem? item = cart.Items.FirstOrDefault(current => current.ProductId == productId);
        if (item is null)
        {
            return false;
        }

        cart.Items.Remove(item);
        return true;
    }

    public void Clear(string cartId)
    {
        var cart = GetCart(cartId);
        cart.Items.Clear();
    }

    public Order Checkout(string cartId, CheckoutRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.CustomerName))
        {
            throw new ArgumentException("Customer name is required.", nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.ShippingAddress))
        {
            throw new ArgumentException("Shipping address is required.", nameof(request));
        }

        ShoppingCart cart = GetCart(cartId);
        if (cart.Items.Count == 0)
        {
            throw new InvalidOperationException("The shopping cart is empty.");
        }

        Order order = new()
        {
            CustomerName = request.CustomerName.Trim(),
            ShippingAddress = request.ShippingAddress.Trim(),
            TotalAmount = cart.Total,
            Status = "Processing",
            OrderedAt = DateTimeOffset.UtcNow
        };

        Order createdOrder = orderService.Create(order);
        Clear(cartId);
        return createdOrder;
    }
}

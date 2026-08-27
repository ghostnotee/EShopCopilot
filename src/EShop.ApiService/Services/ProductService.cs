using EShop.Shared.Models;

namespace EShop.ApiService.Services;

public sealed class ProductService
{
    private readonly List<Product> _items =
    [
        new()
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Name = "Aurora Wireless Headphones",
            Description = "Noise-cancelling over-ear headphones with 30-hour battery life.",
            Price = 129.99m,
            ImageUrl = "https://images.example.com/products/aurora-headphones.jpg"
        },
        new()
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Name = "Cedar Travel Backpack",
            Description = "Water-resistant 28-liter backpack with a padded laptop compartment.",
            Price = 84.50m,
            ImageUrl = "https://images.example.com/products/cedar-backpack.jpg"
        },
        new()
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Name = "Lumen Smart Desk Lamp",
            Description = "Adjustable LED desk lamp with touch controls and USB-C charging.",
            Price = 49.95m,
            ImageUrl = "https://images.example.com/products/lumen-desk-lamp.jpg"
        },
        new()
        {
            Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            Name = "Terra Ceramic Mug Set",
            Description = "Set of four handcrafted ceramic mugs, each holding 350 milliliters.",
            Price = 32.00m,
            ImageUrl = "https://images.example.com/products/terra-mugs.jpg"
        },
        new()
        {
            Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
            Name = "Pulse Fitness Tracker",
            Description = "Lightweight fitness tracker with heart-rate monitoring and sleep tracking.",
            Price = 69.99m,
            ImageUrl = "https://images.example.com/products/pulse-tracker.jpg"
        }
    ];

    public IReadOnlyList<Product> GetAll() => _items.AsReadOnly();

    public Product? GetById(Guid id) => _items.FirstOrDefault(product => product.Id == id);

    public Product Create(Product product)
    {
        product.Id = product.Id == Guid.Empty ? Guid.NewGuid() : product.Id;
        _items.Add(product);
        return product;
    }

    public Product? Update(Guid id, Product product)
    {
        var index = _items.FindIndex(existingProduct => existingProduct.Id == id);
        if (index < 0)
        {
            return null;
        }

        product.Id = id;
        _items[index] = product;
        return product;
    }

    public bool Delete(Guid id)
    {
        var product = GetById(id);
        return product is not null && _items.Remove(product);
    }
}

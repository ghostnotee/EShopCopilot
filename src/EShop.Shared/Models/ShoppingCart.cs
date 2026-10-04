namespace EShop.Shared.Models;

public class ShoppingCart
{
    public List<CartItem> Items { get; set; } = [];

    public decimal Total => Items.Sum(item => item.LineTotal);
}

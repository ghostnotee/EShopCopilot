namespace EShop.Shared.Models;

public class CartItem
{
    public Guid ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; } = 1;

    public decimal LineTotal => UnitPrice * Quantity;
}

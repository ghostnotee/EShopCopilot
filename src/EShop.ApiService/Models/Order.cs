namespace EShop.ApiService.Models;

public class Order
{
    public Guid Id { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public string ShippingAddress { get; set; } = string.Empty;

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTimeOffset OrderedAt { get; set; }
}

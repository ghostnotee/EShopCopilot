namespace EShop.Shared.Models;

public class CheckoutRequest
{
    public string CustomerName { get; set; } = string.Empty;

    public string ShippingAddress { get; set; } = string.Empty;
}

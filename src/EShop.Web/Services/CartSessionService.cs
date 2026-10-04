namespace EShop.Web.Services;

public sealed class CartSessionService
{
    private string? cartId;

    public string GetCartId()
    {
        return cartId ??= Guid.NewGuid().ToString();
    }
}

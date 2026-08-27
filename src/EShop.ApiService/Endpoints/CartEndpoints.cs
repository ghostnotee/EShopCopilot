using EShop.ApiService.Services;
using EShop.Shared.Models;

namespace EShop.ApiService.Endpoints;

public static class CartEndpoints
{
    public static IEndpointRouteBuilder MapCartEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder cart = app.MapGroup("/api/carts")
            .WithTags("Carts");

        cart.MapGet("/", (HttpRequest request, CartService cartService) =>
        {
            string cartId = GetCartId(request);
            return Results.Ok(cartService.GetCart(cartId));
        });

        cart.MapPost("/items", (AddCartItemRequest request, HttpRequest httpRequest, CartService cartService) =>
            Results.Ok(cartService.AddItem(GetCartId(httpRequest), request.ProductId, request.Quantity)));

        cart.MapDelete("/items/{productId:guid}", (Guid productId, HttpRequest request, CartService cartService) =>
        {
            string cartId = GetCartId(request);
            return cartService.RemoveItem(cartId, productId)
                ? Results.NoContent()
                : Results.NotFound();
        });

        cart.MapPost("/checkout", (CheckoutRequest request, HttpRequest httpRequest, CartService cartService) =>
        {
            Order createdOrder = cartService.Checkout(GetCartId(httpRequest), request);
            return Results.Created($"/api/orders/{createdOrder.Id}", createdOrder);
        });

        return app;
    }

    private static string GetCartId(HttpRequest request)
    {
        string? cartId = request.Headers["X-Cart-Id"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(cartId))
        {
            return cartId;
        }

        cartId = request.Query["cartId"].FirstOrDefault();
        return !string.IsNullOrWhiteSpace(cartId)
            ? cartId
            : Guid.NewGuid().ToString();
    }
}

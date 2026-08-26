using EShop.ApiService.Services;
using EShop.Shared.Models;

namespace EShop.ApiService.Endpoints;

public static class CartEndpoints
{
    public static IEndpointRouteBuilder MapCartEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder cart = app.MapGroup("/cart")
            .WithTags("Cart");

        cart.MapGet("/", (HttpRequest request, CartService cartService) =>
        {
            string cartId = GetCartId(request);
            return Results.Ok(cartService.GetCart(cartId));
        });

        cart.MapPost("/items", (AddCartItemRequest request, HttpRequest httpRequest, CartService cartService) =>
        {
            try
            {
                string cartId = GetCartId(httpRequest);
                ShoppingCart updatedCart = cartService.AddItem(cartId, request.ProductId, request.Quantity);
                return Results.Ok(updatedCart);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
            catch (ArgumentOutOfRangeException)
            {
                return Results.BadRequest();
            }
        });

        cart.MapDelete("/items/{productId:guid}", (Guid productId, HttpRequest request, CartService cartService) =>
        {
            string cartId = GetCartId(request);
            return cartService.RemoveItem(cartId, productId)
                ? Results.NoContent()
                : Results.NotFound();
        });

        cart.MapPost("/checkout", (CheckoutRequest request, HttpRequest httpRequest, CartService cartService) =>
        {
            try
            {
                string cartId = GetCartId(httpRequest);
                Order createdOrder = cartService.Checkout(cartId, request);
                return Results.Created($"/orders/{createdOrder.Id}", createdOrder);
            }
            catch (ArgumentException)
            {
                return Results.BadRequest();
            }
            catch (InvalidOperationException)
            {
                return Results.BadRequest();
            }
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

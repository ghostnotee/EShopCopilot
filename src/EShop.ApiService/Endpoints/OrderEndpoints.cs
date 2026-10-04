using EShop.ApiService.Services;
using EShop.Shared.Models;

namespace EShop.ApiService.Endpoints;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder orders = app.MapGroup("/api/orders")
            .WithTags("Orders");

        orders.MapGet("/", (OrderService service) =>
            Results.Ok(service.GetAll()));

        orders.MapGet("/{id:guid}", (Guid id, OrderService service) =>
        {
            Order? order = service.GetById(id);
            return order is null
                ? Results.NotFound()
                : Results.Ok(order);
        });

        orders.MapPost("/", (Order order, OrderService service) =>
        {
            Order createdOrder = service.Create(order);
            return Results.Created($"/api/orders/{createdOrder.Id}", createdOrder);
        });

        orders.MapPut("/{id:guid}", (Guid id, Order order, OrderService service) =>
            service.Update(id, order) is Order updated
                ? Results.Ok(updated)
                : Results.NotFound());

        orders.MapDelete("/{id:guid}", (Guid id, OrderService service) =>
            service.Delete(id)
                ? Results.NoContent()
                : Results.NotFound());

        return app;
    }
}

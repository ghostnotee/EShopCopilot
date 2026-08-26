using EShop.ApiService.Services;
using EShop.Shared.Models;

namespace EShop.ApiService.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder products = app.MapGroup("/products")
            .WithTags("Products");

        products.MapGet("/", (ProductService service) =>
            Results.Ok(service.GetAll()));

        products.MapGet("/{id:guid}", (Guid id, ProductService service) =>
        {
            Product? product = service.GetById(id);
            return product is null
                ? Results.NotFound()
                : Results.Ok(product);
        });

        products.MapPost("/", (Product product, ProductService service) =>
        {
            Product createdProduct = service.Create(product);
            return Results.Created($"/products/{createdProduct.Id}", createdProduct);
        });

        products.MapPut("/{id:guid}", (Guid id, Product product, ProductService service) =>
            service.Update(id, product)
                ? Results.NoContent()
                : Results.NotFound());

        products.MapDelete("/{id:guid}", (Guid id, ProductService service) =>
            service.Delete(id)
                ? Results.NoContent()
                : Results.NotFound());

        return app;
    }
}

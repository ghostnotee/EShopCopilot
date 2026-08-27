using EShop.ApiService.Services;
using EShop.Shared.Models;

namespace EShop.ApiService.Endpoints;

public static class CustomerEndpoints
{
    public static IEndpointRouteBuilder MapCustomerEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/customers")
            .WithTags("Customers");

        group.MapGet("/", (CustomerService service) =>
            Results.Ok(service.GetAll()));

        group.MapGet("/{id:guid}", (Guid id, CustomerService service) =>
            service.GetById(id) is Customer customer
                ? Results.Ok(customer)
                : Results.NotFound());

        group.MapPost("/", (Customer customer, CustomerService service) =>
        {
            Customer created = service.Create(customer);
            return Results.Created($"/api/customers/{created.Id}", created);
        });

        group.MapPut("/{id:guid}", (Guid id, Customer customer, CustomerService service) =>
            service.Update(id, customer) is Customer updated
                ? Results.Ok(updated)
                : Results.NotFound());

        group.MapDelete("/{id:guid}", (Guid id, CustomerService service) =>
            service.Delete(id)
                ? Results.NoContent()
                : Results.NotFound());

        return app;
    }
}

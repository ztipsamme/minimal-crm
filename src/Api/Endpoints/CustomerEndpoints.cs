using System;
using api.Services;
using api.Models;

namespace api.Endpoints;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this WebApplication app)
    {
        var customerGroup = app.MapGroup("/api/customer");

        customerGroup.MapGet("", async (CustomerService service) =>
            Results.Ok(await service.GetAll()));

        customerGroup.MapGet("{vendorId}/{id}", async (CustomerService service, string vendorId, string id) =>
        {
            var customer = await service.GetById(vendorId, id);
            return customer is null ? Results.NotFound() : Results.Ok(customer);
        });

        customerGroup.MapPost("", async (CustomerService service, Customer newCustomer) =>
        {
            var created = await service.Create(newCustomer);
            return Results.Ok(created);
        });
    }
}
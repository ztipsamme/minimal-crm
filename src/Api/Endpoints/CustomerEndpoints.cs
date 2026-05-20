using System;
using Api.Services;
using Api.Dto;
using Domain.Models;

namespace Api.Endpoints;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this WebApplication app)
    {
        var customerGroup = app.MapGroup("/Api/customer");

        customerGroup.MapGet("", async (CustomerService service, [AsParameters] CustomerQueryParams queryParams) =>
        {
            var customers = await service.GetAll(queryParams);
            return Results.Ok(customers);
        });

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

        customerGroup.MapPost("{oldVendorId}/{id}/change-vendor", async (CustomerService service, CustomerChangeVendorDto newVendor, string oldVendorId, string id) =>
        {
            var created = await service.ChangeVendor(newVendor, oldVendorId, id);
            return Results.Ok(created);
        });

        customerGroup.MapPatch("{vendorId}/{id}", async (CustomerService service, PatchCustomerDto updatedCustomer, string vendorId, string id) =>
        {
            var patched = await service.Patch(updatedCustomer, vendorId, id);
            return patched is null ? Results.NotFound() : Results.Ok(patched);

        });

        customerGroup.MapDelete("{vendorId}/{id}", async (CustomerService service, string vendorId, string id) =>
        {
            var deleted = await service.Delete(vendorId, id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}
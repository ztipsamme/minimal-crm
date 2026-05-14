using System;
using api.Services;
using api.Models;

namespace api.Endpoints;

public static class VendorEndpoints
{
    public static void MapVendorEndpoints(this WebApplication app)
    {
        var vendorGroup = app.MapGroup("/api/vendor");

        vendorGroup.MapGet("", async (VendorService service) =>
            Results.Ok(await service.GetAll()));

        vendorGroup.MapGet("{id}", async (VendorService service, string id) =>
        {
            var vendor = await service.GetById(id);
            return vendor is null ? Results.NotFound() : Results.Ok(vendor);
        });

        vendorGroup.MapPost("", async (VendorService service, Vendor newVendor) =>
        {
            var created = await service.Create(newVendor);
            return Results.Ok(created);
        });
    }
}

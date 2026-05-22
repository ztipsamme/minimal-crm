using System;
using Api.Dto;
using Api.Services;
using Domain.Models;

namespace Api.Endpoints
{
    public static class UserEndpoints
    {
        public static WebApplication MapUserEndpoints(this WebApplication app)
        {
            var customerGroup = app.MapGroup("/api/users");

            customerGroup.MapGet("", GetAll);
            customerGroup.MapGet("{id}", GetById);

            customerGroup.MapPost("customers", CreateCustomer);
            customerGroup.MapPost("vendors", CreateVendor);

            customerGroup.MapPatch("{id}", Patch);

            customerGroup.MapDelete("{id}", Delete);

            return app;
        }

        public static async Task<IResult> GetAll(UserService service, [AsParameters] UserQueryParams queryParams)
        {
            var users = await service.GetAll(queryParams);
            return Results.Ok(users);
        }


        public static async Task<IResult> GetById(UserService service, string vendorId, string id)
        {
            var users = await service.GetById(vendorId, id);
            return users is null ? Results.NotFound() : Results.Ok(users);
        }

        public static async Task<IResult> CreateCustomer(UserService service, CustomerDto dto)
        {
            var user = new Customer(dto.Name, dto.PhoneNumber, dto.Email, dto.Title, dto.Address, dto.VendorId);

            return Results.Ok(await service.Create(user));
        }

        public static async Task<IResult> CreateVendor(UserService service, VendorDto dto)
        {
            var user = new Vendor(dto.Name, dto.PhoneNumber, dto.Email);

            return Results.Ok(await service.Create(user));
        }

        public static async Task<IResult> Patch(UserService service, PatchUserDto dto, string vendorId, string id)
        {
            var patched = await service.Patch(dto, vendorId, id);
            return patched is null ? Results.NotFound() : Results.Ok(patched);
        }

        public static async Task<IResult> Delete(UserService service, string vendorId, string id)
        {
            var deleted = await service.Delete(vendorId, id);
            return deleted ? Results.NoContent() : Results.NotFound();
        }
    }
}
using System;
using Api.Dto;
using Api.Helpers;
using Domain.Models;
using Microsoft.Azure.Cosmos;

namespace Api.Services;

public class CustomerService
{
    private readonly Container _container;
    private readonly CustomerQueryHelper _queryHelper;

    public CustomerService(CosmosService cosmos, CustomerQueryHelper queryHelper)
    {
        _container = cosmos.GetContainer("customers");
        _queryHelper = queryHelper;
    }

    public async Task<IEnumerable<Customer>> GetAll(CustomerQueryParams queryParams)
    {
        return await _queryHelper.ExecuteQuery(_container, queryParams);
    }

    public async Task<Customer?> GetById(string vendorId, string id)
    {
        try
        {
            var response = await _container.ReadItemAsync<Customer>(
                id,
                new PartitionKey(vendorId)
            );

            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<Customer?> Create(Customer customer)
    {
        try
        {
            var response = await _container.CreateItemAsync(customer, new PartitionKey(customer.VendorId));

            return response.Resource;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }

    public async Task<Customer?> ChangeVendor(CustomerChangeVendorDto newVendor, string oldVendorId, string id)
    {
        try
        {
            var customer = await GetById(oldVendorId, id);
            if (customer is null) return null;

            customer.VendorId = newVendor.NewVendorId;
            customer.UpdatedAt = DateTime.UtcNow;

            var response = await _container.CreateItemAsync(customer, new PartitionKey(newVendor.NewVendorId));

            await Delete(oldVendorId, id);

            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<Customer?> Patch(PatchCustomerDto patched, string vendorId, string id)
    {
        try
        {
            var customer = await GetById(vendorId, id);
            if (customer is null) return null;

            var patchOperations = PatchBuilder.From(patched);

            var response = await _container.PatchItemAsync<Customer>(
                id,
                new PartitionKey(vendorId),
                patchOperations
            );

            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<bool> Delete(string vendorId, string id)
    {
        try
        {
            var customer = await GetById(vendorId, id);
            if (customer is null) return false;

            var response = await _container.DeleteItemAsync<Customer>(
                id,
                new PartitionKey(vendorId)
            );

            return true;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
    }

}

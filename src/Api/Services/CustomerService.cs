using System;
using api.Dto;
using api.Helpers;
using api.Models;
using Microsoft.Azure.Cosmos;

namespace api.Services;

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
}

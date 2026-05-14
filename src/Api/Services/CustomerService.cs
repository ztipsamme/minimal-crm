using System;
using api.Models;
using Microsoft.Azure.Cosmos;

namespace api.Services;

public class CustomerService
{
    private readonly Container _container;

    public CustomerService(CosmosService cosmos)
    {
        _container = cosmos.GetContainer("customers");
    }

    public async Task<IEnumerable<Customer>> GetAll()
    {
        var query = _container.GetItemQueryIterator<Customer>();

        var results = new List<Customer>();

        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response);
        }

        return results;
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

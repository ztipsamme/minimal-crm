using System;
using api.Models;
using Microsoft.Azure.Cosmos;

namespace api.Services;

public class VendorService
{
    private readonly Container _container;

    public VendorService(CosmosService cosmos)
    {
        _container = cosmos.GetContainer("vendors");
    }

    public async Task<IEnumerable<Vendor>> GetAll()
    {
        var query = _container.GetItemQueryIterator<Vendor>();

        var results = new List<Vendor>();

        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response);
        }

        return results;
    }

    public async Task<Vendor?> GetById(string id)
    {
        try
        {
            var response = await _container.ReadItemAsync<Vendor>(
                id,
                new PartitionKey(id)
            );

            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<Vendor?> Create(Vendor vendor)
    {
        Console.WriteLine($"Id: {vendor.Id}");

        try
        {
            var response = await _container.CreateItemAsync(
                vendor,
                new PartitionKey(vendor.Id)
            );

            return response.Resource;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }
}

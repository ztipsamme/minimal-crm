using System;
using Microsoft.Azure.Cosmos;

namespace api.Services;

public class CosmosService
{
    private readonly CosmosClient _client;
    private readonly Database _database;

    public CosmosService(IConfiguration config)
    {
        _client = new CosmosClient(
            config["Cosmos:ConnectionString"]);
        _database = _client.GetDatabase("minimalcrm");

    }

    public Container GetContainer(string name)
         => _database.GetContainer(name);
}

using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.CosmosDB;
using Microsoft.Extensions.Logging;
using Domain.Models;

namespace Company.Function;

public class NewCustomerFunction
{
    private readonly ILogger<NewCustomerFunction> _logger;

    public NewCustomerFunction(ILogger<NewCustomerFunction> logger)
    {
        _logger = logger;
    }

    [Function("EmailTrigger")]
    public void Run([CosmosDBTrigger(
        databaseName: "minimalcrm",
        containerName: "customer",
        Connection = "Cosmos:ConnectionString",
        LeaseContainerName = "leases",
        CreateLeaseContainerIfNotExists = true)] IReadOnlyList<Customer> input)
    {
        if (input != null && input.Count > 0)
        {
            // _logger.LogInformation("Documents modified: " + input.Count);
            // _logger.LogInformation("First document Id: " + input[0].id);
        }
    }
}
using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Domain.Models;
using Application.Services;
using Infrastructure.Services;
using SendGrid.Helpers.Mail;

namespace Company.Function;

public class NewCustomerFunction
{
    private readonly ILogger<NewCustomerFunction> _logger;
    private readonly EmailService _emailService;
    private readonly UserService _userService;

    public NewCustomerFunction(ILogger<NewCustomerFunction> logger, EmailService emailService, UserService userService)
    {
        _logger = logger;
        _emailService = emailService;
        _userService = userService;
    }

    [Function("EmailTrigger")]
    public async Task Run([CosmosDBTrigger(
        databaseName: "minimalcrm",
        containerName: "users",
        Connection = "Cosmos:ConnectionString",
        LeaseContainerName = "leases",
        CreateLeaseContainerIfNotExists = true)] IReadOnlyList<Customer> input)
    {
        if (input != null && input.Count > 0)
        {
            _logger.LogInformation("Documents modified: " + input.Count);

            foreach (var user in input)
            {
                if (user.Role != "customer")
                    continue;

                var vendor = await _userService.GetById(user.VendorId, user.VendorId);
                if (vendor is null) return;

                var subject = "New customer assigned";
                var textBody = $@"
                        Hello {vendor.Name},

                        You have a new customer:

                        Namn: {user.Name}
                        Email: {user.Email}
                        Telefon: {user.PhoneNumber}
                        ";

                await _emailService.SendEmail(
                    new EmailAddress(vendor.Email, vendor.Name),
                    subject,
                    textBody
                );
            }

            _logger.LogInformation("First document Id: " + input[0].Id);
        }
    }
}
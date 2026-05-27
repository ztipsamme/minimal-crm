using System;
using System.Collections.Generic;
using Application.Services;
using Domain.Models;
using Infrastructure.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;

namespace Functions;

public class EmailTrigger
{
    private readonly ILogger<EmailTrigger> _logger;

    private readonly EmailService _emailService;
    private readonly UserService _userService;

    public EmailTrigger(ILogger<EmailTrigger> logger, EmailService emailService, UserService userService)
    {
        _logger = logger;
        _emailService = emailService;
        _userService = userService;
    }

    [Function("EmailTrigger")]
    public async Task Run([CosmosDBTrigger(
        databaseName: "minimalcrm",
        containerName: "users",
        Connection = "ConnectionString",
        LeaseContainerName = "leases",
        CreateLeaseContainerIfNotExists = true)] IReadOnlyList<User> input)
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
                var textBody = $"You have a new customer";

                var htmlBody = @$"
                <p>Hello {vendor.Name},</p>

                <p>You have a new customer:</p>

                <table style='border-collapse: collapse;'>
                    <tr>
                        <td><b>Name</b></td>
                        <td>{user.Name}</td>
                    </tr>
                    <tr>
                        <td><b>Email</b></td>
                        <td>{user.Email}</td>
                    </tr>
                    <tr>
                        <td><b>Phone</b></td>
                        <td>{user.PhoneNumber}</td>
                    </tr>
                </table>

                <table style='border-collapse: collapse;'>
                    <caption style='text-align: left;'><b>Address</b>></caption>
                    <tr>
                        <td><b>Street</b></td>
                        <td>{user.Address.Street}</td>
                    </tr>
                    <tr>
                        <td><b>City</b></td>
                        <td>{user.Address.City}</td>
                    </tr>
                    <tr>
                        <td><b>Postal code</b></td>
                        <td>{user.Address.PostalCode}</td>
                    </tr>
                    <tr>
                        <td><b>Country</b></td>
                        <td>{user.Address.Country}</td>
                    </tr>
                </table>
                ";

                var to = new EmailAddress(vendor.Email, vendor.Name);

                await _emailService.SendEmail(
                    to,
                    subject,
                    textBody,
                    htmlBody
                );
                _logger.LogInformation("To:" + vendor.Email);
            }

            _logger.LogInformation("First document Id: " + input[0].Id);
        }
    }
}
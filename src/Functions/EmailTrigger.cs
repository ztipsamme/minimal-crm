using System;
using System.Collections.Generic;
using Application.Services;
using Domain.Models;
using Infrastructure.Helpers;
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
                // "New" if CreatedAt and UpdatedAt are almost identical.
                // Small timestamp differences can occur in Cosmos DB.
                var isNew = Math.Abs((user.UpdatedAt - user.CreatedAt).TotalMilliseconds) < 10;

                if (user.Role != "customer")
                    continue;

                var vendor = await _userService.GetById(user.VendorId, user.VendorId);
                if (vendor is null)
                {
                    _logger.LogWarning($"Vendor not found for VendorId: {user.VendorId}");
                    return;
                }

                var to = new EmailAddress(vendor.Email, vendor.Name);
                var email = EmailBuilder.Build(isNew, vendor, user);

                await _emailService.SendEmail(
                    to,
                    email.subject,
                    email.textBody,
                    email.htmlBody
                );

                _logger.LogInformation("To:" + vendor.Email);
            }

            _logger.LogInformation("First document Id: " + input[0].Id);
        }
    }
}
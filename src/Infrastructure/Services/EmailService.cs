using System;
using Application.Contracts;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class EmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly EmailOptions _options;
        private readonly SendGridClient _client;

        public EmailService(ILogger<EmailService> logger, IOptions<EmailOptions> options)
        {
            _logger = logger;
            _options = options.Value;

            var clientOptions = new SendGridClientOptions
            {
                ApiKey = _options.ApiKey
            };

            if (_options.UseEuRegion)
                clientOptions.SetDataResidency("eu");

            _client = new SendGridClient(clientOptions);
        }

        public async Task SendEmail(
            EmailAddress to,
            string subject,
            string textBody,
            string? htmlBody = null)
        {
            var from = new EmailAddress(_options.FromEmail, _options.FromName);

            _logger.LogInformation("From:" + _options.FromEmail);


            var msg = MailHelper.CreateSingleEmail(
                from,
                to,
                subject,
                textBody,
                htmlBody ?? textBody
            );

            var response = await _client.SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Body.ReadAsStringAsync();
                throw new InvalidOperationException(
                    $"Failed to send email via SendGrid. Status: {response.StatusCode}, Body: {body}"
                );
            }
        }
    }
}
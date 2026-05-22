using System;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Services
{
    public class EmailService
    {
        private readonly EmailOptions _options;
        private readonly SendGridClient _client;

        public EmailService(IOptions<EmailOptions> options)
        {
            _options = options.Value;

            var clientOptions = new SendGridClientOptions
            {
                ApiKey = _options.SendGridApiKey
            };

            if (_options.UseEuRegion)
            {
                clientOptions.SetDataResidency("eu");
            }

            _client = new SendGridClient(clientOptions);
        }

        public async Task SendEmail(
            EmailAddress to,
            string subject,
            string textBody,
            string? htmlBody = null)
        {
            var from = new EmailAddress("e.spitz@hotmail.com", "Emma API");

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


    public class EmailOptions
    {
        public string FromEmail { get; set; } = default!;
        public string FromName { get; set; } = default!;
        public string SendGridApiKey { get; set; } = default!;
        public bool UseEuRegion { get; set; }
    }
}
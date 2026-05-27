using System;

namespace Application.Contracts
{
    public class EmailOptions
    {
        public string FromEmail { get; set; } = default!;
        public string FromName { get; set; } = default!;
        public string SendGridApiKey { get; set; } = default!;
        public bool UseEuRegion { get; set; }
    }
}
namespace NotificationService.Infrastructure.Sms
{
    using Microsoft.Extensions.Logging;
    using NotificationService.Application.Contracts.Infrastructure;
    using NotificationService.Application.Common;

    public class TwilioSmsProvider : ISmsProvider
    {
        private readonly ILogger<TwilioSmsProvider> _logger;

        public TwilioSmsProvider(ILogger<TwilioSmsProvider> logger)
        {
            _logger = logger;
        }

        public Task<string> SendSmsAsync(string to, string message)
        {
            _logger.LogInformation("--- Sending SMS (Placeholder) ---");
            _logger.LogInformation("To: {To}", RedactionHelper.MaskPhone(to));
            _logger.LogInformation("Message: [Redacted for logging]");
            _logger.LogInformation("--- SMS Sent ---");

            return Task.FromResult("Success");
        }
    }
}
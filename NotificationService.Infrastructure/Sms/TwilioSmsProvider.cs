namespace NotificationService.Infrastructure.Sms
{
    using Microsoft.Extensions.Logging;
    using NotificationService.Application.Contracts.Infrastructure;

    /// <summary>
    /// Placeholder implementation for an SMS Provider.
    /// Logs to console instead of making a real API call.
    /// </summary>
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
            _logger.LogInformation("To: {To}", to);
            _logger.LogInformation("Message: {Message}", message);
            _logger.LogInformation("--- SMS Sent ---");

            return Task.FromResult("Success");
        }
    }
}
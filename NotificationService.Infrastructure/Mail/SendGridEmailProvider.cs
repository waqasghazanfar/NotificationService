namespace NotificationService.Infrastructure.Mail
{
    using Microsoft.Extensions.Logging;
    using NotificationService.Application.Contracts.Infrastructure;

    /// <summary>
    /// Placeholder implementation for an Email Provider.
    /// Logs to console instead of making a real API call.
    /// </summary>
    public class SendGridEmailProvider : IEmailProvider
    {
        private readonly ILogger<SendGridEmailProvider> _logger;

        public SendGridEmailProvider(ILogger<SendGridEmailProvider> logger)
        {
            _logger = logger;
        }

        public Task<string> SendEmailAsync(string to, string subject, string body)
        {
            _logger.LogInformation("--- Sending Email (Placeholder) ---");
            _logger.LogInformation("To: {To}", to);
            _logger.LogInformation("Subject: {Subject}", subject);
            _logger.LogInformation("Body: {Body}", body);
            _logger.LogInformation("--- Email Sent ---");

            return Task.FromResult("Success");
        }
    }
}
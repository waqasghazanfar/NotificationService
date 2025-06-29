namespace NotificationService.Infrastructure.Mail
{
    using MailKit.Net.Smtp;
    using Microsoft.Extensions.Logging;
    using MimeKit;
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

        public async Task<string> SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("My App", "noreply@example.com")); // Sender Name & Email
                message.To.Add(MailboxAddress.Parse(to));
                message.Subject = subject;

                message.Body = new TextPart("plain")
                {
                    Text = body
                };

                using var client = new SmtpClient();

                // Hardcoded SMTP settings for now
                await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("hammad.hassan@purelogics.com", "jdmlpulryhkaekca");

                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation("Email sent to {To} successfully.", to);
                return "Success";
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {To}", to);
                return "Failed";
            }
        }
    }
}
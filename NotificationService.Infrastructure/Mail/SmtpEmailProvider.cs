namespace NotificationService.Infrastructure.Mail
{
    using MailKit.Net.Smtp;
    using MailKit.Security;
    using Microsoft.Extensions.Logging;
    using MimeKit;
    using NotificationService.Application.Common;
    using NotificationService.Application.Contracts.Infrastructure;
    using NotificationService.Application.Contracts.Persistence;
    using NotificationService.Domain.Entities;

    public class SmtpEmailProvider : IEmailProvider
    {
        private readonly ILogger<SmtpEmailProvider> _logger;
        private readonly ISmtpSettingRepository _smtpSettingRepository;

        public SmtpEmailProvider(ILogger<SmtpEmailProvider> logger, ISmtpSettingRepository smtpSettingRepository)
        {
            _logger = logger;
            _smtpSettingRepository = smtpSettingRepository;
        }

        public async Task<string> SendEmailAsync(string to, string subject, string body, Guid? smtpSettingId)
        {
            SmtpSetting? smtpSetting;

            if (smtpSettingId.HasValue)
            {
                _logger.LogInformation("Attempting to use specified SMTP setting {SmtpSettingId}", smtpSettingId.Value);
                smtpSetting = await _smtpSettingRepository.GetByIdAsync(smtpSettingId.Value);
            }
            else
            {
                _logger.LogInformation("No SMTP setting specified, falling back to default.");
                smtpSetting = await _smtpSettingRepository.GetDefaultAsync();
            }

            if (smtpSetting == null || !smtpSetting.IsActive)
            {
                _logger.LogError("No active SMTP setting found for this request.");
                return "Failed: No active SMTP configuration could be found.";
            }

            _logger.LogInformation("Attempting to send email via {Host} to {Recipient}", smtpSetting.Host, RedactionHelper.MaskEmail(to));

            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(smtpSetting.FromName, smtpSetting.FromAddress));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;

                var builder = new BodyBuilder { HtmlBody = body };
                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(smtpSetting.Host, smtpSetting.Port, smtpSetting.EnableSsl);
                await smtp.AuthenticateAsync(smtpSetting.Username, smtpSetting.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                _logger.LogInformation("Email successfully sent to {Recipient} via {Host}", RedactionHelper.MaskEmail(to), smtpSetting.Host);
                return "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Email provider failed to send email.");
                return $"Failed: {ex.Message}";
            }
        }
    }
}
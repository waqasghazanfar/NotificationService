namespace NotificationService.Application.Contracts.Infrastructure
{
    public interface IEmailProvider
    {
        Task<string> SendEmailAsync(string to, string subject, string body, Guid? smtpSettingId);
    }
}
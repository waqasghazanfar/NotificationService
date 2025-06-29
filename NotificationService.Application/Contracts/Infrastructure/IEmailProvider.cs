namespace NotificationService.Application.Contracts.Infrastructure
{
    public interface IEmailProvider
    {
        Task<string> SendEmailAsync(Application.DTOs.NotificationRequestDto payload, string subject, string body, Guid? smtpSettingId);
    }
}
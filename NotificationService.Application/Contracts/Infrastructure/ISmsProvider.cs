namespace NotificationService.Application.Contracts.Infrastructure
{
    public interface ISmsProvider
    {
        Task<string> SendSmsAsync(string to, string message);
    }
}
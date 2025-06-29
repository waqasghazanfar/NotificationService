namespace NotificationService.Application.Contracts.Persistence
{
    public interface INotificationLogRepository : IAsyncRepository<Domain.Entities.NotificationLog>
    {
        Task<IReadOnlyList<Domain.Entities.NotificationLog>> GetHistoryByRecipientAsync(string recipient);
        Task<Domain.Entities.NotificationLog?> GetNextDueNotificationAsync();
        Task<IReadOnlyList<Domain.Entities.NotificationLog>> GetByCorrelationIdFilteredAsync(Guid correlationId, DateTime? startDate, DateTime? endDate, string? eventName, Guid? smtpSettingId, string? userId); // <-- MODIFIED
    }
}
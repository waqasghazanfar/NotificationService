namespace NotificationService.Application.Contracts.Infrastructure
{
    using NotificationService.Domain.Entities;

    public interface IInMemoryNotificationQueue
    {
        ValueTask EnqueueAsync(NotificationLog workItem);
        ValueTask<NotificationLog> DequeueAsync(CancellationToken cancellationToken);
    }
}
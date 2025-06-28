namespace NotificationService.Application.Features.Notifications.Commands.SendNotification
{
    using MediatR;
    using System.Text.Json;
    using NotificationService.Application.Contracts.Persistence;
    using NotificationService.Application.Contracts.Infrastructure;
    using NotificationService.Domain.Entities;

    public class SendNotificationCommandHandler : IRequestHandler<SendNotificationCommand, Guid>
    {
        private readonly INotificationLogRepository _notificationLogRepository;
        private readonly IInMemoryNotificationQueue _queue;

        public SendNotificationCommandHandler(INotificationLogRepository notificationLogRepository, IInMemoryNotificationQueue queue)
        {
            _notificationLogRepository = notificationLogRepository;
            _queue = queue;
        }

        public async Task<Guid> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
        {
            var correlationId = request.NotificationRequest.Metadata.CorrelationId;

            // This logic is now more robust to handle multiple channels
            var channels = request.NotificationRequest.Overrides.Channels;
            if (channels == null || !channels.Any())
            {
                // In a real scenario, you might have default channels based on event type
                // or user preferences. For now, we'll assume at least one is provided.
                throw new ArgumentException("No channels specified for notification.");
            }

            foreach (var channel in channels)
            {
                string recipient = "Unknown";
                if (channel.Equals("Email", StringComparison.OrdinalIgnoreCase))
                {
                    recipient = request.NotificationRequest.Recipient.Email?.To.FirstOrDefault() ?? "Unknown";
                }
                else if (channel.Equals("Sms", StringComparison.OrdinalIgnoreCase))
                {
                    recipient = request.NotificationRequest.Recipient.PhoneNumber ?? "Unknown";
                }

                var log = new NotificationLog
                {
                    Id = Guid.NewGuid(),
                    CorrelationId = correlationId,
                    EventName = request.NotificationRequest.Event.Name,
                    Channel = channel,
                    Recipient = recipient,
                    Status = "Queued",
                    Payload = JsonSerializer.Serialize(request.NotificationRequest),
                    CreatedAtUtc = DateTime.UtcNow
                };

                await _notificationLogRepository.AddAsync(log);

                // Enqueue each log for background processing
                await _queue.EnqueueAsync(log);
            }

            return correlationId;
        }
    }
}
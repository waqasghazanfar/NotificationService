namespace NotificationService.Application.Features.Notifications.Commands.SendNotification
{
    using MediatR;
    using System.Text.Json;
    using NotificationService.Application.Contracts.Persistence;
    using NotificationService.Domain.Entities;
    using NotificationService.Application.Contracts.Infrastructure;

    public class SendNotificationCommandHandler : IRequestHandler<SendNotificationCommand, Guid>
    {
        private readonly INotificationLogRepository _notificationLogRepository;
        private readonly IInMemoryNotificationQueue _signalQueue;

        public SendNotificationCommandHandler(INotificationLogRepository notificationLogRepository, IInMemoryNotificationQueue signalQueue)
        {
            _notificationLogRepository = notificationLogRepository;
            _signalQueue = signalQueue;
        }

        public async Task<Guid> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
        {
            var commandRequest = request.NotificationRequest;
            var correlationId = commandRequest.Metadata.CorrelationId;

            var channels = commandRequest.Overrides.Channels;
            if (channels == null || !channels.Any())
            {
                throw new ArgumentException("No channels specified for notification.");
            }

            foreach (var channel in channels)
            {
                string recipient = "Unknown";
                if (channel.Equals("Email", StringComparison.OrdinalIgnoreCase))
                {
                    recipient = commandRequest.Recipient.Email?.To.FirstOrDefault() ?? "Unknown";
                }
                else if (channel.Equals("Sms", StringComparison.OrdinalIgnoreCase))
                {
                    recipient = commandRequest.Recipient.PhoneNumber ?? "Unknown";
                }

                var log = new NotificationLog
                {
                    Id = Guid.NewGuid(),
                    CorrelationId = correlationId,
                    UserId = commandRequest.Recipient.UserId,
                    EventName = commandRequest.Event.Name,
                    Channel = channel,
                    Recipient = recipient,
                    Status = "Queued",
                    Priority = commandRequest.Metadata.Priority,
                    ScheduleAtUtc = commandRequest.Metadata.ScheduleAtUtc,
                    SmtpSettingId = commandRequest.Metadata.SmtpSettingId,
                    Payload = JsonSerializer.Serialize(commandRequest),
                    CreatedAtUtc = DateTime.UtcNow
                };

                await _notificationLogRepository.AddAsync(log);
            }

            await _signalQueue.EnqueueAsync(new NotificationLog());

            return correlationId;
        }
    }
}
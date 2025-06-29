namespace NotificationService.Application.Features.Notifications.Commands.SendNotification
{
    using MediatR;
    using NotificationService.Application.DTOs;

    public class SendNotificationCommand : IRequest<Guid>
    {
        public NotificationRequestDto NotificationRequest { get; set; } = new();
    }
}
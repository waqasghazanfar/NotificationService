using MediatR;
using NotificationService.Application.DTOs;

namespace NotificationService.Application.Features.Notifications.Commands.SendNotification
{
    public class SendNotificationCommand : IRequest<Guid>
    {
        public NotificationRequestDto NotificationRequest { get; set; } = new();
    }
}

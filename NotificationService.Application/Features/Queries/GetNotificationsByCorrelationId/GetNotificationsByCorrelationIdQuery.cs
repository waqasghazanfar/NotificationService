namespace NotificationService.Application.Features.Notifications.Queries.GetNotificationsByCorrelationId
{
    using MediatR;
    using NotificationService.Application.DTOs;

    public class GetNotificationsByCorrelationIdQuery : IRequest<List<NotificationLogDto>>
    {
        public Guid CorrelationId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? EventName { get; set; }
        public Guid? SmtpSettingId { get; set; }
        public string? UserId { get; set; } 
    }
}
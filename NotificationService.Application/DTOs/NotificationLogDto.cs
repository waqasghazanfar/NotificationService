namespace NotificationService.Application.DTOs
{
    public class NotificationLogDto
    {
        public Guid Id { get; set; }
        public Guid CorrelationId { get; set; }
        public string? UserId { get; set; } // <-- ADDED
        public string EventName { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public string Recipient { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = "Low";
        public DateTime? ScheduleAtUtc { get; set; }
        public string? ProviderResponse { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? ProcessedAtUtc { get; set; }
    }
}

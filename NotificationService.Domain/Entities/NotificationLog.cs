﻿namespace NotificationService.Domain.Entities
{
    /// <summary>
    /// Represents a log entry for a single notification attempt.
    /// This entity serves as an audit trail for all outgoing communications.
    /// </summary>
    public class NotificationLog
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
        public Guid? SmtpSettingId { get; set; }
        public string Payload { get; set; } = string.Empty;
        public string? TemplateContent { get; set; }
        public string? ProviderResponse { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? ProcessedAtUtc { get; set; }
    }
}

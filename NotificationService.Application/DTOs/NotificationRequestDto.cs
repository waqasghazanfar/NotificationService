namespace NotificationService.Application.DTOs
{
    public class NotificationRequestDto
    {
        public RecipientDto Recipient { get; set; } = new();
        public EventDto Event { get; set; } = new();
        public MetadataDto Metadata { get; set; } = new();
        public OverrideDto Overrides { get; set; } = new();
    }

    public class RecipientDto
    {
        public string? UserId { get; set; }
        public EmailDto? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public List<string>? PushTokens { get; set; }
    }

    public class EmailDto
    {
        public List<string> To { get; set; } = new();
        public List<string>? Cc { get; set; }
        public List<string>? Bcc { get; set; }
    }

    public class EventDto
    {
        public string Name { get; set; } = string.Empty;
        public Dictionary<string, object> Data { get; set; } = new();
        public string Locale { get; set; } = "en-GB";
    }

    public class MetadataDto
    {
        public Guid CorrelationId { get; set; } = Guid.NewGuid();
        public string Priority { get; set; } = "Low";
        public DateTime? ScheduleAtUtc { get; set; }
        public Guid? SmtpSettingId { get; set; }
    }

    public class OverrideDto
    {
        public List<string> Channels { get; set; } = new();
    }
}
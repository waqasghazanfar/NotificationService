using NotificationService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }

    public class OverrideDto
    {
        public List<ChannelType> Channels { get; set; } = new();
    }
}

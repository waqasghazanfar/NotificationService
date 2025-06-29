namespace NotificationService.Domain.Entities
{
    /// <summary>
    /// Represents a reusable notification template.
    /// Templates allow for consistent messaging and easy content updates.
    /// </summary>
    public class Template
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public string Locale { get; set; } = string.Empty;
        public string? Subject { get; set; }
        public string Body { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? UpdatedAtUtc { get; set; }
    }
}
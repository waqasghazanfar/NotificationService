namespace NotificationService.Application.DTOs
{
    public class TemplateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public string Locale { get; set; } = string.Empty;
        public string? Subject { get; set; }
        public string Body { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
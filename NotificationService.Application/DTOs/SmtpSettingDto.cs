namespace NotificationService.Application.DTOs
{
    public class SmtpSettingDto
    {
        public Guid Id { get; set; }
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FromAddress { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;
        public bool EnableSsl { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Domain.Entities
{
    /// <summary>
    /// Represents a single SMTP server configuration.
    /// </summary>
    public class SmtpSetting
    {
        public Guid Id { get; set; }
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FromAddress { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;
        public bool EnableSsl { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
    }
}

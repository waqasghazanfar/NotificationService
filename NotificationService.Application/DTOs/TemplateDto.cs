using NotificationService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.DTOs
{
    public class TemplateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ChannelType Channel { get; set; } = ChannelType.Email;
        public string Locale { get; set; } = string.Empty;
        public string? Subject { get; set; }
        public string Body { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
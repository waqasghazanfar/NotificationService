﻿namespace NotificationService.Application.Features.SmtpSettings.Commands.CreateSmtpSetting
{
    using MediatR;

    public class CreateSmtpSettingCommand : IRequest<Guid>
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FromAddress { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;
        public bool EnableSsl { get; set; }
        public bool IsDefault { get; set; } = false;
        public bool IsActive { get; set; }
    }
}
namespace NotificationService.Application.Features.Templates.Commands.CreateTemplate
{
    using MediatR;
    using NotificationService.Domain.Enums;

    public class CreateTemplateCommand : IRequest<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public ChannelType Channel { get; set; } = ChannelType.Email;
        public string Locale { get; set; } = string.Empty;
        public string? Subject { get; set; }
        public string Body { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
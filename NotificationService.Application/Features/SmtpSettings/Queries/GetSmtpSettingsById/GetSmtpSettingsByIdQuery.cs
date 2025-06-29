namespace NotificationService.Application.Features.SmtpSettings.Queries.GetTemplatesList
{
    using MediatR;
    using NotificationService.Application.Features.SmtpSettings.Queries;

    public class GetSmtpSettingsByIdQuery : IRequest<SmtpSettingsListVm>
    {
        public Guid Id { get; set; }
    }
}
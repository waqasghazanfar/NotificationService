namespace NotificationService.Application.Features.SmtpSettings.Queries.GetSmtpSettingsList
{
    using MediatR;

    public class GetSmtpSettingsListQuery : IRequest<List<SmtpSettingsListVm>>
    {
    }
}

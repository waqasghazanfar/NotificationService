namespace NotificationService.Application.Features.SmtpSettings.Commands.UpdateSmtpSetting
{
    using MediatR;
    using NotificationService.Application.Contracts.Persistence;
    using NotificationService.Domain;

    public class UpdateSmtpSettingCommandHandler : IRequestHandler<UpdateSmtpSettingCommand>
    {
        private readonly ISmtpSettingRepository _smtpSettingRepository;

        public UpdateSmtpSettingCommandHandler(ISmtpSettingRepository smtpSettingRepository)
        {
            _smtpSettingRepository = smtpSettingRepository;
        }

        public async Task Handle(UpdateSmtpSettingCommand request, CancellationToken cancellationToken)
        {
            var settingToUpdate = await _smtpSettingRepository.GetByIdAsync(request.Id);
            if (settingToUpdate == null)
            {
                throw new Exception($"SmtpSetting with id {request.Id} not found.");
            }

            if (request.IsDefault && !settingToUpdate.IsDefault)
            {
                await _smtpSettingRepository.UnsetAllDefaultsAsync();
            }

            settingToUpdate.Host = request.Host;
            settingToUpdate.Port = request.Port;
            settingToUpdate.Username = request.Username;
            settingToUpdate.FromAddress = request.FromAddress;
            settingToUpdate.FromName = request.FromName;
            settingToUpdate.EnableSsl = request.EnableSsl;
            settingToUpdate.IsDefault = request.IsDefault;
            settingToUpdate.IsActive = request.IsActive;

            if (!string.IsNullOrEmpty(request.Password))
            {
                settingToUpdate.Password = request.Password.Encrypt();
            }

            await _smtpSettingRepository.UpdateAsync(settingToUpdate);
        }
    }
}

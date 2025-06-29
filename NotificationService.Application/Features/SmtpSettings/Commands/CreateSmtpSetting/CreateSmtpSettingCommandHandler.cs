namespace NotificationService.Application.Features.SmtpSettings.Commands.CreateSmtpSetting
{
    using AutoMapper;
    using MediatR;
    using NotificationService.Application.Contracts.Persistence;
    using NotificationService.Domain.Entities;

    public class CreateSmtpSettingCommandHandler : IRequestHandler<CreateSmtpSettingCommand, Guid>
    {
        private readonly ISmtpSettingRepository _smtpSettingRepository;
        private readonly IMapper _mapper;

        public CreateSmtpSettingCommandHandler(IMapper mapper, ISmtpSettingRepository smtpSettingRepository)
        {
            _mapper = mapper;
            _smtpSettingRepository = smtpSettingRepository;
        }

        public async Task<Guid> Handle(CreateSmtpSettingCommand request, CancellationToken cancellationToken)
        {
            if (request.IsDefault)
            {
                await _smtpSettingRepository.UnsetAllDefaultsAsync();
            }

            var smtpSetting = _mapper.Map<SmtpSetting>(request);
            smtpSetting = await _smtpSettingRepository.AddAsync(smtpSetting);
            return smtpSetting.Id;
        }
    }
}

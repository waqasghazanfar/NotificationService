namespace NotificationService.Application.Features.SmtpSettings.Queries.GetTemplatesList
{
    using AutoMapper;
    using MediatR;
    using NotificationService.Application.Contracts.Persistence;

    public class GetSmtpSettingsByIdQueryHandler : IRequestHandler<GetSmtpSettingsByIdQuery, SmtpSettingsListVm>
    {
        private readonly IAsyncRepository<Domain.Entities.SmtpSetting> _smtpSettingsRepository;
        private readonly IMapper _mapper;

        public GetSmtpSettingsByIdQueryHandler(IMapper mapper, IAsyncRepository<Domain.Entities.SmtpSetting> smtpSettingsRepository)
        {
            _mapper = mapper;
            _smtpSettingsRepository = smtpSettingsRepository;
        }

        public async Task<SmtpSettingsListVm> Handle(GetSmtpSettingsByIdQuery request, CancellationToken cancellationToken)
        {
            var smtpSettings = await _smtpSettingsRepository.GetByIdAsync(request.Id);
            return _mapper.Map<SmtpSettingsListVm>(smtpSettings);
        }
    }
}
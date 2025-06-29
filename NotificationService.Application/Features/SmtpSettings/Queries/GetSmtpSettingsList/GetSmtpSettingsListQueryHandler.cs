namespace NotificationService.Application.Features.Templates.Queries.GetTemplatesList
{
    using AutoMapper;
    using MediatR;
    using NotificationService.Application.Contracts.Persistence;
    using NotificationService.Application.Features.SmtpSettings.Queries;
    using NotificationService.Application.Features.SmtpSettings.Queries.GetSmtpSettingsList;

    public class GetSmtpSettingsListQueryHandler : IRequestHandler<GetSmtpSettingsListQuery, List<SmtpSettingsListVm>>
    {
        private readonly IAsyncRepository<Domain.Entities.SmtpSetting> _smtpSettingsRepository;
        private readonly IMapper _mapper;

        public GetSmtpSettingsListQueryHandler(IMapper mapper, IAsyncRepository<Domain.Entities.SmtpSetting> smtpSettingsRepository)
        {
            _mapper = mapper;
            _smtpSettingsRepository = smtpSettingsRepository;
        }

        public async Task<List<SmtpSettingsListVm>> Handle(GetSmtpSettingsListQuery request, CancellationToken cancellationToken)
        {
            var allSmtpSettings = await _smtpSettingsRepository.ListAllAsync();
            return _mapper.Map<List<SmtpSettingsListVm>>(allSmtpSettings);
        }
    }
}
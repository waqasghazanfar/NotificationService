namespace NotificationService.Application.Features.Templates.Commands.CreateTemplate
{
    using AutoMapper;
    using MediatR;
    using NotificationService.Application.Contracts.Persistence;
    using NotificationService.Domain.Entities;

    public class CreateTemplateCommandHandler : IRequestHandler<CreateTemplateCommand, Guid>
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly IMapper _mapper;

        public CreateTemplateCommandHandler(IMapper mapper, ITemplateRepository templateRepository)
        {
            _mapper = mapper;
            _templateRepository = templateRepository;
        }

        public async Task<Guid> Handle(CreateTemplateCommand request, CancellationToken cancellationToken)
        {
            var template = _mapper.Map<Template>(request);
            template.CreatedAtUtc = DateTime.UtcNow;
            template = await _templateRepository.AddAsync(template);
            return template.Id;
        }
    }
}

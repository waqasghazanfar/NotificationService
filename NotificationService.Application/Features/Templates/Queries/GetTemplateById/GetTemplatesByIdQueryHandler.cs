namespace NotificationService.Application.Features.Templates.Queries.GetTemplatesList
{
    using AutoMapper;
    using MediatR;
    using NotificationService.Application.Contracts.Persistence;

    public class GetTemplatesByIdQueryHandler : IRequestHandler<GetTemplatesByIdQuery, TemplateListVm>
    {
        private readonly IAsyncRepository<Domain.Entities.Template> _templateRepository;
        private readonly IMapper _mapper;

        public GetTemplatesByIdQueryHandler(IMapper mapper, IAsyncRepository<Domain.Entities.Template> templateRepository)
        {
            _mapper = mapper;
            _templateRepository = templateRepository;
        }

        public async Task<TemplateListVm> Handle(GetTemplatesByIdQuery request, CancellationToken cancellationToken)
        {
            var template = await _templateRepository.GetByIdAsync(request.Id);
            return _mapper.Map<TemplateListVm>(template);
        }
    }
}
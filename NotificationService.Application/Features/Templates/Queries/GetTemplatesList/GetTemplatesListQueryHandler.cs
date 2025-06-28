namespace NotificationService.Application.Features.Templates.Queries.GetTemplatesList
{
    using AutoMapper;
    using MediatR;
    using NotificationService.Application.Contracts.Persistence;

    public class GetTemplatesListQueryHandler : IRequestHandler<GetTemplatesListQuery, List<TemplateListVm>>
    {
        private readonly IAsyncRepository<Domain.Entities.Template> _templateRepository;
        private readonly IMapper _mapper;

        public GetTemplatesListQueryHandler(IMapper mapper, IAsyncRepository<Domain.Entities.Template> templateRepository)
        {
            _mapper = mapper;
            _templateRepository = templateRepository;
        }

        public async Task<List<TemplateListVm>> Handle(GetTemplatesListQuery request, CancellationToken cancellationToken)
        {
            var allTemplates = await _templateRepository.ListAllAsync();
            return _mapper.Map<List<TemplateListVm>>(allTemplates);
        }
    }
}
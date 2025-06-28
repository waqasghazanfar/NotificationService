namespace NotificationService.Application.Features.Templates.Queries.GetTemplatesList
{
    using MediatR;

    public class GetTemplatesListQuery : IRequest<List<TemplateListVm>>
    {
    }
}
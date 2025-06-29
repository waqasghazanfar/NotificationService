namespace NotificationService.Application.Features.Templates.Queries.GetTemplatesList
{
    using MediatR;

    public class GetTemplatesByIdQuery : IRequest<TemplateListVm>
    {
        public Guid Id { get; set; }
    }
}
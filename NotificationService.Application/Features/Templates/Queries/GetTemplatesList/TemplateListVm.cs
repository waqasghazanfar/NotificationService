namespace NotificationService.Application.Features.Templates.Queries.GetTemplatesList
{
    public class TemplateListVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public string Locale { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}

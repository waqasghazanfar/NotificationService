namespace NotificationService.Application.Contracts.Persistence
{
    public interface ITemplateRepository : IAsyncRepository<Domain.Entities.Template>
    {
        Task<Domain.Entities.Template?> GetTemplateAsync(string name, string channel, string locale);
    }
}
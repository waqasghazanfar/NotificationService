namespace NotificationService.Application.Contracts.Persistence
{
    public interface ISmtpSettingRepository : IAsyncRepository<Domain.Entities.SmtpSetting>
    {
        Task<Domain.Entities.SmtpSetting?> GetDefaultAsync();
        Task UnsetAllDefaultsAsync();
    }
}
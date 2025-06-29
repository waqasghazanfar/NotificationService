namespace NotificationService.Infrastructure.Persistence.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using NotificationService.Application.Contracts.Persistence;
    using NotificationService.Domain.Entities;

    public class SmtpSettingRepository : BaseRepository<SmtpSetting>, ISmtpSettingRepository
    {
        public SmtpSettingRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<SmtpSetting?> GetDefaultAsync()
        {
            return await _dbContext.SmtpSettings
                .FirstOrDefaultAsync(s => s.IsDefault && s.IsActive);
        }

        public async Task UnsetAllDefaultsAsync()
        {
            await _dbContext.SmtpSettings
                .Where(s => s.IsDefault)
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.IsDefault, false));
        }
    }
}
namespace NotificationService.Infrastructure.Persistence.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using NotificationService.Application.Contracts.Persistence;
    using NotificationService.Domain.Entities;

    public class NotificationLogRepository : BaseRepository<NotificationLog>, INotificationLogRepository
    {
        public NotificationLogRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IReadOnlyList<NotificationLog>> GetHistoryByRecipientAsync(string recipient)
        {
            return await _dbContext.NotificationLogs
                .Where(nl => nl.Recipient == recipient)
                .OrderByDescending(nl => nl.CreatedAtUtc)
                .ToListAsync();
        }
    }
}
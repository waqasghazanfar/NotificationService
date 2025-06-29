namespace NotificationService.Infrastructure.Persistence.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using NotificationService.Application.Contracts.Persistence;
    using NotificationService.Domain.Entities;
    using System.Linq;

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

        public async Task<IReadOnlyList<NotificationLog>> GetByCorrelationIdFilteredAsync(Guid correlationId, DateTime? startDate, DateTime? endDate, string? eventName, Guid? smtpSettingId, string? userId)
        {
            var query = _dbContext.NotificationLogs.AsQueryable();

            query = query.Where(nl => nl.CorrelationId == correlationId);

            if (startDate.HasValue)
            {
                query = query.Where(nl => nl.CreatedAtUtc >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(nl => nl.CreatedAtUtc <= endDate.Value);
            }

            if (!string.IsNullOrEmpty(eventName))
            {
                query = query.Where(nl => nl.EventName == eventName);
            }

            if (smtpSettingId.HasValue)
            {
                query = query.Where(nl => nl.SmtpSettingId == smtpSettingId.Value);
            }

            if (!string.IsNullOrEmpty(userId)) // <-- ADDED FILTER
            {
                query = query.Where(nl => nl.UserId == userId);
            }

            return await query.OrderBy(nl => nl.CreatedAtUtc).ToListAsync();
        }

        public async Task<NotificationLog?> GetNextDueNotificationAsync()
        {
            var now = DateTime.UtcNow;

            var dueNotifications = await _dbContext.NotificationLogs
                .Where(nl => nl.Status == "Queued" && (nl.ScheduleAtUtc == null || nl.ScheduleAtUtc <= now))
                .ToListAsync();

            return dueNotifications
                .OrderBy(nl => nl.Priority == "High" ? 1 : nl.Priority == "Medium" ? 2 : 3)
                .ThenBy(nl => nl.CreatedAtUtc)
                .FirstOrDefault();
        }
    }
}
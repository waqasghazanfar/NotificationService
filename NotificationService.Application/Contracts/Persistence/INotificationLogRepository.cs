using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Contracts.Persistence
{
    public interface INotificationLogRepository : IAsyncRepository<Domain.Entities.NotificationLog>
    {
        Task<IReadOnlyList<Domain.Entities.NotificationLog>> GetHistoryByRecipientAsync(string recipient);
    }
}
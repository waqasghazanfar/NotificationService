using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Contracts.Persistence
{
    public interface ITemplateRepository : IAsyncRepository<Domain.Entities.Template>
    {
        Task<Domain.Entities.Template?> GetTemplateAsync(string name, string channel, string locale);
    }
}

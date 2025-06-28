using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Contracts.Infrastructure
{
    public interface ISmsProvider
    {
        Task<string> SendSmsAsync(string to, string message);
    }
}

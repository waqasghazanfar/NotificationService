using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Contracts.Infrastructure
{
    public interface IEmailProvider
    {
        Task<string> SendEmailAsync(string to, string subject, string body);
    }
}
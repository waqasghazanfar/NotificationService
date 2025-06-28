namespace NotificationService.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using NotificationService.Application.Contracts.Infrastructure;
    using NotificationService.Application.Contracts.Persistence;
    using NotificationService.Infrastructure.Mail;
    using NotificationService.Infrastructure.Persistence;
    using NotificationService.Infrastructure.Persistence.Repositories;
    using NotificationService.Infrastructure.Queue;
    using NotificationService.Infrastructure.Sms;

    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
            services.AddScoped<INotificationLogRepository, NotificationLogRepository>();
            services.AddScoped<ITemplateRepository, TemplateRepository>();

            services.AddTransient<IEmailProvider, SendGridEmailProvider>();
            services.AddTransient<ISmsProvider, TwilioSmsProvider>();

            // Register in-memory queue and processor
            services.AddSingleton<IInMemoryNotificationQueue, InMemoryNotificationQueue>();
            services.AddHostedService<QueuedNotificationProcessor>();

            return services;
        }
    }
}
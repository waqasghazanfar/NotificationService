namespace NotificationService.Infrastructure.Queue
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using HandlebarsDotNet;
    using System.Text.Json;
    using NotificationService.Application.Contracts.Persistence;
    using NotificationService.Application.Contracts.Infrastructure;

    public class QueuedNotificationProcessor : BackgroundService
    {
        private readonly ILogger<QueuedNotificationProcessor> _logger;
        private readonly IServiceProvider _serviceProvider;

        public QueuedNotificationProcessor(ILogger<QueuedNotificationProcessor> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Queued Notification Processor is running.");

            await ProcessQueue(stoppingToken);
        }

        private async Task ProcessQueue(CancellationToken stoppingToken)
        {
            // We must create a new scope for each processing loop to resolve scoped services
            // like DbContext and repositories correctly.
            using var scope = _serviceProvider.CreateScope();
            var queue = scope.ServiceProvider.GetRequiredService<IInMemoryNotificationQueue>();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var workItem = await queue.DequeueAsync(stoppingToken);

                    using var processingScope = _serviceProvider.CreateScope();
                    await ProcessWorkItem(processingScope.ServiceProvider, workItem);
                }
                catch (OperationCanceledException)
                {
                    // Prevent throwing if stoppingToken was signaled
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred processing a notification.");
                }
            }
        }

        private async Task ProcessWorkItem(IServiceProvider sp, Domain.Entities.NotificationLog log)
        {
            var logRepo = sp.GetRequiredService<INotificationLogRepository>();
            var templateRepo = sp.GetRequiredService<ITemplateRepository>();

            log.Status = "Processing";
            await logRepo.UpdateAsync(log);

            // Deserialize payload to get template info
            var payload = JsonSerializer.Deserialize<Application.DTOs.NotificationRequestDto>(log.Payload);
            if (payload == null)
            {
                log.Status = "Failed";
                log.ProviderResponse = "Invalid payload.";
                await logRepo.UpdateAsync(log);
                return;
            }

            // Fetch template
            var template = await templateRepo.GetTemplateAsync(payload.Event.Name, log.Channel, payload.Event.Locale);
            if (template == null)
            {
                log.Status = "Failed";
                log.ProviderResponse = $"Template not found for {payload.Event.Name}/{log.Channel}/{payload.Event.Locale}";
                await logRepo.UpdateAsync(log);
                return;
            }

            // Render template
            var compiledTemplate = Handlebars.Compile(template.Body);
            var renderedBody = compiledTemplate(payload.Event.Data);
            log.TemplateContent = renderedBody;

            // Send via provider
            string response = "Provider not found for channel.";
            try
            {
                if (log.Channel.Equals("Email", StringComparison.OrdinalIgnoreCase))
                {
                    var provider = sp.GetRequiredService<IEmailProvider>();
                    response = await provider.SendEmailAsync(log.Recipient, template.Subject ?? "Notification", renderedBody);
                }
                else if (log.Channel.Equals("Sms", StringComparison.OrdinalIgnoreCase))
                {
                    var provider = sp.GetRequiredService<ISmsProvider>();
                    response = await provider.SendSmsAsync(log.Recipient, renderedBody);
                }

                log.Status = "Sent";
                log.ProviderResponse = response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Provider failed to send notification {Id}", log.Id);
                log.Status = "Failed";
                log.ProviderResponse = ex.Message;
            }

            log.ProcessedAtUtc = DateTime.UtcNow;
            await logRepo.UpdateAsync(log);
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Queued Notification Processor is stopping.");
            await base.StopAsync(stoppingToken);
        }
    }
}
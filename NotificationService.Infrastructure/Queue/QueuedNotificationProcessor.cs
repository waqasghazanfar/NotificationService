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
        private readonly IInMemoryNotificationQueue _signalQueue;

        public QueuedNotificationProcessor(
            ILogger<QueuedNotificationProcessor> logger,
            IServiceProvider serviceProvider,
            IInMemoryNotificationQueue signalQueue)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _signalQueue = signalQueue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Queued Notification Processor is running.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _signalQueue.DequeueAsync(stoppingToken);
                    await ProcessDatabaseQueue(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred in the queue processing loop.");
                    await Task.Delay(5000, stoppingToken);
                }
            }
        }

        private async Task ProcessDatabaseQueue(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Checking database for due notifications.");

            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var logRepo = scope.ServiceProvider.GetRequiredService<INotificationLogRepository>();

                var log = await logRepo.GetNextDueNotificationAsync();

                if (log == null)
                {
                    _logger.LogInformation("No due notifications found. Waiting for next signal.");
                    break;
                }

                await ProcessWorkItem(scope.ServiceProvider, log);
            }
        }

        private async Task ProcessWorkItem(IServiceProvider sp, Domain.Entities.NotificationLog log)
        {
            var logRepo = sp.GetRequiredService<INotificationLogRepository>();
            var templateRepo = sp.GetRequiredService<ITemplateRepository>();

            _logger.LogInformation("Processing notification {Id} with priority {Priority}", log.Id, log.Priority);

            log.Status = "Processing";
            await logRepo.UpdateAsync(log);

            var payload = JsonSerializer.Deserialize<Application.DTOs.NotificationRequestDto>(log.Payload);
            if (payload == null)
            {
                log.Status = "Failed";
                log.ProviderResponse = "Invalid payload.";
                await logRepo.UpdateAsync(log);
                return;
            }

            var template = await templateRepo.GetTemplateAsync(payload.Event.Name, log.Channel, payload.Event.Locale);
            if (template == null)
            {
                log.Status = "Failed";
                log.ProviderResponse = $"Template not found for {payload.Event.Name}/{log.Channel}/{payload.Event.Locale}";
                await logRepo.UpdateAsync(log);
                return;
            }

            var compiledTemplate = Handlebars.Compile(template.Body);
            var renderedBody = compiledTemplate(payload.Event.Data);
            log.TemplateContent = renderedBody;
            renderedBody = ReplacePlaceholders(renderedBody, payload.Event.Data);

            string response = "Provider not found for channel.";
            try
            {
                if (log.Channel.Equals("Email", StringComparison.OrdinalIgnoreCase))
                {
                    var provider = sp.GetRequiredService<IEmailProvider>();
                    response = await provider.SendEmailAsync(payload, template.Subject ?? "Notification", renderedBody, log.SmtpSettingId);
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

        public static string ReplacePlaceholders(string body, Dictionary<string, object> placeholders)
        {
            if (string.IsNullOrEmpty(body))
            {
                return body;
            }

            if (placeholders == null || placeholders.Count == 0)
            {
                return body; // No placeholders to replace
            }

            string updatedBody = body;

            foreach (var entry in placeholders)
            {
                // Construct the exact placeholder string to search for: [[Key]]
                string placeholderToFind = $"[[{entry.Key}]]";

                // Replace all occurrences of the placeholder with its corresponding value
                // Using Replace is efficient for simple string replacements.
                // If you need more complex regex patterns, Regex.Replace would be used.
                updatedBody = updatedBody.Replace(placeholderToFind, entry.Value.ToString());
            }

            return updatedBody;
        }
    }
}
namespace NotificationService.Application.UnitTests.Mocks
{
    using Moq;
    using NotificationService.Application.Contracts.Persistence;
    using NotificationService.Domain.Entities;
    using NotificationService.Domain.Enums;

    public static class MockRepository
    {
        public static Mock<ITemplateRepository> GetTemplateRepository()
        {
            var templates = new List<Template>
            {
                new Template
                {
                    Id = Guid.NewGuid(),
                    Name = "TestEvent",
                    Channel = ChannelType.Email,
                    Locale = "en-GB",
                    Subject = "Test Subject",
                    Body = "Hello {{name}}",
                    IsActive = true,
                    CreatedAtUtc = DateTime.UtcNow
                },
                new Template
                {
                    Id = Guid.NewGuid(),
                    Name = "TestEvent",
                    Channel = ChannelType.Sms,
                    Locale = "en-GB",
                    Body = "Hi {{name}}",
                    IsActive = true,
                    CreatedAtUtc = DateTime.UtcNow
                }
            };

            var mockTemplateRepository = new Mock<ITemplateRepository>();

            mockTemplateRepository.Setup(repo => repo.ListAllAsync()).ReturnsAsync(templates);

            mockTemplateRepository.Setup(repo => repo.AddAsync(It.IsAny<Template>())).ReturnsAsync((Template template) =>
            {
                template.Id = Guid.NewGuid();
                templates.Add(template);
                return template;
            });

            return mockTemplateRepository;
        }

        public static Mock<INotificationLogRepository> GetNotificationLogRepository()
        {
            var mockNotificationLogRepository = new Mock<INotificationLogRepository>();
            return mockNotificationLogRepository;
        }
    }
}
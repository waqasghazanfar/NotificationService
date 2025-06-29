namespace NotificationService.Application.UnitTests.Features.Notifications.Commands
{
    using Moq;
    using Xunit;
    using FluentAssertions;
    using NotificationService.Application.Contracts.Persistence;
    using NotificationService.Application.Contracts.Infrastructure;
    using NotificationService.Application.Features.Notifications.Commands.SendNotification;
    using NotificationService.Application.DTOs;
    using NotificationService.Domain.Entities;
    using NotificationService.Application.UnitTests.Mocks;
    using MockRepository = Mocks.MockRepository;

    public class SendNotificationCommandHandlerTests
    {
        private readonly Mock<INotificationLogRepository> _mockLogRepo;
        private readonly Mock<IInMemoryNotificationQueue> _mockQueue;
        private readonly SendNotificationCommandHandler _handler;

        public SendNotificationCommandHandlerTests()
        {
            _mockLogRepo = MockRepository.GetNotificationLogRepository();
            _mockQueue = new Mock<IInMemoryNotificationQueue>();
            _handler = new SendNotificationCommandHandler(_mockLogRepo.Object, _mockQueue.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateLogAndSignalQueue()
        {
            var command = new SendNotificationCommand
            {
                NotificationRequest = new NotificationRequestDto
                {
                    Recipient = new RecipientDto { UserId = "user-123", Email = new EmailDto { To = new List<string> { "test@example.com" } } },
                    Event = new EventDto { Name = "TestEvent" },
                    Overrides = new OverrideDto { Channels = new List<string> { "Email" } },
                    Metadata = new MetadataDto { Priority = "High", ScheduleAtUtc = DateTime.UtcNow.AddHours(1) }
                }
            };
            NotificationLog? capturedLog = null;

            _mockLogRepo.Setup(r => r.AddAsync(It.IsAny<NotificationLog>()))
                .Callback<NotificationLog>(log => capturedLog = log)
                .ReturnsAsync((NotificationLog log) => log);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().Be(command.NotificationRequest.Metadata.CorrelationId);
            _mockLogRepo.Verify(r => r.AddAsync(It.IsAny<NotificationLog>()), Times.Once);
            _mockQueue.Verify(q => q.EnqueueAsync(It.IsAny<NotificationLog>()), Times.Once);
            capturedLog.Should().NotBeNull();
            capturedLog?.UserId.Should().Be("user-123");
            capturedLog?.Priority.Should().Be("High");
            capturedLog?.ScheduleAtUtc.Should().Be(command.NotificationRequest.Metadata.ScheduleAtUtc);
        }
    }
}

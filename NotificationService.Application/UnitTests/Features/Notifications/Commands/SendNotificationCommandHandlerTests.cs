using MediatR;

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
    using NotificationService.Domain.Enums;

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
        public async Task Handle_ShouldCreateLogAndEnqueue_ForSingleChannel()
        {
            // Arrange
            var command = new SendNotificationCommand
            {
                NotificationRequest = new NotificationRequestDto
                {
                    Recipient = new RecipientDto { Email = new EmailDto { To = new List<string> { "test@example.com" } } },
                    Event = new EventDto { Name = "TestEvent" },
                    Overrides = new OverrideDto { Channels = new List<ChannelType> { ChannelType.Email } }
                }
            };
            NotificationLog? capturedLog = null;

            _mockLogRepo.Setup(r => r.AddAsync(It.IsAny<NotificationLog>()))
                .Callback<NotificationLog>(log => capturedLog = log)
                .ReturnsAsync((NotificationLog log) => log);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(command.NotificationRequest.Metadata.CorrelationId);

            _mockLogRepo.Verify(r => r.AddAsync(It.IsAny<NotificationLog>()), Times.Once);
            _mockQueue.Verify(q => q.EnqueueAsync(It.IsAny<NotificationLog>()), Times.Once);

            capturedLog.Should().NotBeNull();
            capturedLog?.Status.Should().Be("Queued");
            capturedLog?.Channel.Should().Be(ChannelType.Email);
            capturedLog?.Recipient.Should().Be("test@example.com");
        }

        [Fact]
        public async Task Handle_ShouldCreateMultipleLogsAndEnqueue_ForMultipleChannels()
        {
            // Arrange
            var command = new SendNotificationCommand
            {
                NotificationRequest = new NotificationRequestDto
                {
                    Recipient = new RecipientDto
                    {
                        Email = new EmailDto { To = new List<string> { "test@example.com" } },
                        PhoneNumber = "1234567890"
                    },
                    Event = new EventDto { Name = "TestEvent" },
                    Overrides = new OverrideDto { Channels = new List<ChannelType> { ChannelType.Email, ChannelType.Sms } }
                }
            };
            var capturedLogs = new List<NotificationLog>();

            _mockLogRepo.Setup(r => r.AddAsync(It.IsAny<NotificationLog>()))
                .Callback<NotificationLog>(log => capturedLogs.Add(log))
                .ReturnsAsync((NotificationLog log) => log);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(command.NotificationRequest.Metadata.CorrelationId);

            _mockLogRepo.Verify(r => r.AddAsync(It.IsAny<NotificationLog>()), Times.Exactly(2));
            _mockQueue.Verify(q => q.EnqueueAsync(It.IsAny<NotificationLog>()), Times.Exactly(2));

            capturedLogs.Should().HaveCount(2);
            capturedLogs.Should().Contain(l => l.Channel == ChannelType.Email && l.Recipient == "test@example.com");
            capturedLogs.Should().Contain(l => l.Channel == ChannelType.Sms && l.Recipient == "1234567890");
        }
    }
}
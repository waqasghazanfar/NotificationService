namespace NotificationService.Application.UnitTests.Features.Notifications.Queries
{
    using AutoMapper;
    using Moq;
    using Xunit;
    using FluentAssertions;
    using NotificationService.Application.Contracts.Persistence;
    using NotificationService.Application.Mappings;
    using NotificationService.Application.Features.Notifications.Queries.GetNotificationsByCorrelationId;
    using NotificationService.Domain.Entities;
    using NotificationService.Application.UnitTests.Mocks;
    using NotificationService.Application.DTOs;
    using System.Collections.Generic;
    using MockRepository = Mocks.MockRepository;
    using NotificationService.Application.Features.Templates.Queries.GetTemplatesList;

    public class GetNotificationsByCorrelationIdQueryHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<INotificationLogRepository> _mockRepo;

        public GetNotificationsByCorrelationIdQueryHandlerTests()
        {
            _mockRepo = MockRepository.GetNotificationLogRepository();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task Handle_ReturnsCorrectlyFilteredLogs()
        {
            var correlationId = Guid.NewGuid();
            var logs = new List<NotificationLog>();

            _mockRepo.Setup(r => r.GetByCorrelationIdFilteredAsync(correlationId, It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<Guid?>(), It.IsAny<string>()))
                .ReturnsAsync((Guid cId, DateTime? sd, DateTime? ed, string? ev, Guid? sId, string? uId) =>
                {
                    var filteredLogs = new List<NotificationLog>
                    {
                        new NotificationLog { CorrelationId = correlationId, EventName = "Event1", UserId = "user-123", CreatedAtUtc = DateTime.UtcNow }
                    };
                    if (uId == "user-123") return filteredLogs;
                    return new List<NotificationLog>();
                });

            var handler = new GetNotificationsByCorrelationIdQueryHandler(_mapper, _mockRepo.Object);
            var query = new GetNotificationsByCorrelationIdQuery { CorrelationId = correlationId, UserId = "user-123" };

            var result = await handler.Handle(query, CancellationToken.None);
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().UserId.Should().Be("user-123");
            _mockRepo.Verify(r => r.GetByCorrelationIdFilteredAsync(correlationId, null, null, null, null, "user-123"), Times.Once);
        }
    }
}
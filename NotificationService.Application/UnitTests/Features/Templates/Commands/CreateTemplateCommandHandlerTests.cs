namespace NotificationService.Application.UnitTests.Features.Templates.Commands
{
    using AutoMapper;
    using Moq;
    using Xunit;
    using FluentAssertions;
    using NotificationService.Application.Contracts.Persistence;
    using Application.Mappings;
    using NotificationService.Application.Features.Templates.Commands.CreateTemplate;
    using NotificationService.Domain.Entities;
    using NotificationService.Application.UnitTests.Mocks;
    using MockRepository = Mocks.MockRepository;
    using NotificationService.Domain.Enums;

    public class CreateTemplateCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ITemplateRepository> _mockRepo;

        public CreateTemplateCommandHandlerTests()
        {
            _mockRepo = MockRepository.GetTemplateRepository();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task Handle_ValidTemplate_AddedToRepo()
        {
            // Arrange
            var handler = new CreateTemplateCommandHandler(_mapper, _mockRepo.Object);
            var command = new CreateTemplateCommand() { Name = "NewTemplate", Channel = ChannelType.Email, Locale = "en-GB", Body = "Body" };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            var allTemplates = await _mockRepo.Object.ListAllAsync();
            result.Should().NotBe(Guid.Empty);
            allTemplates.Should().HaveCount(3);
            allTemplates.Should().Contain(t => t.Name == "NewTemplate");
        }
    }
}

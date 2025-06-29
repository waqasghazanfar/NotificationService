namespace NotificationService.Application.UnitTests.Features.SmtpSettings.Commands
{
    using AutoMapper;
    using Moq;
    using Xunit;
    using FluentAssertions;
    using NotificationService.Application.Contracts.Persistence;
    using Application.Mappings;
    using NotificationService.Application.Features.SmtpSettings.Commands.CreateSmtpSetting;
    using NotificationService.Domain.Entities;
    using NotificationService.Application.UnitTests.Mocks;
    using MockRepository = Mocks.MockRepository;

    public class CreateSmtpSettingCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ISmtpSettingRepository> _mockRepo;

        public CreateSmtpSettingCommandHandlerTests()
        {
            _mockRepo = MockRepository.GetSmtpSettingRepository();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task Handle_WhenIsDefaultIsTrue_UnsetAllDefaultsIsCalled()
        {
            var handler = new CreateSmtpSettingCommandHandler(_mapper, _mockRepo.Object);
            var command = new CreateSmtpSettingCommand() { Host = "smtp.test.com", IsDefault = true };

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<SmtpSetting>()))
               .ReturnsAsync((SmtpSetting s) => { s.Id = Guid.NewGuid(); return s; });

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().NotBe(Guid.Empty);
            _mockRepo.Verify(r => r.UnsetAllDefaultsAsync(), Times.Once);
            _mockRepo.Verify(r => r.AddAsync(It.Is<SmtpSetting>(s => s.Host == "smtp.test.com")), Times.Once);
        }
    }
}
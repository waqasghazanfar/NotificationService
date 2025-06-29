namespace NotificationService.Application.UnitTests.Features.SmtpSettings.Commands
{
    using Moq;
    using Xunit;
    using FluentAssertions;
    using NotificationService.Application.Contracts.Persistence;
    using NotificationService.Application.Features.SmtpSettings.Commands.UpdateSmtpSetting;
    using NotificationService.Domain.Entities;
    using NotificationService.Application.UnitTests.Mocks;
    using MockRepository = Mocks.MockRepository;

    public class UpdateSmtpSettingCommandHandlerTests
    {
        private readonly Mock<ISmtpSettingRepository> _mockRepo;

        public UpdateSmtpSettingCommandHandlerTests()
        {
            _mockRepo = MockRepository.GetSmtpSettingRepository();
        }

        [Fact]
        public async Task Handle_WhenSettingNewDefault_UnsetAllDefaultsIsCalled()
        {
            var settingId = Guid.NewGuid();
            var existingSetting = new SmtpSetting { Id = settingId, IsDefault = false, Host = "old.host" };

            _mockRepo.Setup(r => r.GetByIdAsync(settingId)).ReturnsAsync(existingSetting);

            var handler = new UpdateSmtpSettingCommandHandler(_mockRepo.Object);
            var command = new UpdateSmtpSettingCommand { Id = settingId, Host = "new.host", IsDefault = true };

            await handler.Handle(command, CancellationToken.None);

            _mockRepo.Verify(r => r.UnsetAllDefaultsAsync(), Times.Once);
            _mockRepo.Verify(r => r.UpdateAsync(It.Is<SmtpSetting>(s => s.IsDefault == true && s.Host == "new.host")), Times.Once);
        }
    }
}
namespace NotificationService.Application.UnitTests.Features.Templates.Queries
{
    using AutoMapper;
    using Moq;
    using Xunit;
    using FluentAssertions;
    using NotificationService.Application.Contracts.Persistence;
    using NotificationService.Application.Mappings;
    using NotificationService.Application.Features.Templates.Queries.GetTemplatesList;
    using NotificationService.Domain.Entities;
    using NotificationService.Application.UnitTests.Mocks;
    using MockRepository = Mocks.MockRepository;

    public class GetTemplatesListQueryHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ITemplateRepository> _mockRepo;

        public GetTemplatesListQueryHandlerTests()
        {
            _mockRepo = MockRepository.GetTemplateRepository();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task GetTemplatesListTest()
        {
            var handler = new GetTemplatesListQueryHandler(_mapper, _mockRepo.Object);
            var result = await handler.Handle(new GetTemplatesListQuery(), CancellationToken.None);

            result.Should().BeOfType<List<TemplateListVm>>();
            result.Should().HaveCount(2);
        }
    }
}

namespace NotificationService.Application.Features.Notifications.Queries.GetNotificationsByCorrelationId
{
    using AutoMapper;
    using MediatR;
    using NotificationService.Application.Contracts.Persistence;
    using NotificationService.Application.DTOs;

    public class GetNotificationsByCorrelationIdQueryHandler : IRequestHandler<GetNotificationsByCorrelationIdQuery, List<NotificationLogDto>>
    {
        private readonly INotificationLogRepository _notificationLogRepository;
        private readonly IMapper _mapper;

        public GetNotificationsByCorrelationIdQueryHandler(IMapper mapper, INotificationLogRepository notificationLogRepository)
        {
            _mapper = mapper;
            _notificationLogRepository = notificationLogRepository;
        }

        public async Task<List<NotificationLogDto>> Handle(GetNotificationsByCorrelationIdQuery request, CancellationToken cancellationToken)
        {
            var logs = await _notificationLogRepository.GetByCorrelationIdFilteredAsync(
                request.CorrelationId,
                request.StartDate,
                request.EndDate,
                request.EventName,
                request.SmtpSettingId,
                request.UserId); // <-- PASS USER ID

            return _mapper.Map<List<NotificationLogDto>>(logs);
        }
    }
}
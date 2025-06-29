namespace NotificationService.Api.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NotificationService.Api.Authentication;
    using NotificationService.Application.DTOs;
    using NotificationService.Application.Features.Notifications.Commands.SendNotification;
    using NotificationService.Application.Features.Notifications.Queries.GetNotificationsByCorrelationId;

    [ApiController]
    [Route("api/[controller]")]
    [ApiKey] // Apply the API Key authentication to the whole controller
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("/api/notify")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequestDto notificationRequest)
        {
            var command = new SendNotificationCommand { NotificationRequest = notificationRequest };
            var correlationId = await _mediator.Send(command);

            return Accepted(new { CorrelationId = correlationId });
        }

        [HttpGet("{correlationId}", Name = "GetNotificationsByCorrelationId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<NotificationLogDto>>> GetNotificationsByCorrelationId(
            Guid correlationId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] string? eventName,
            [FromQuery] Guid? smtpSettingId,
            [FromQuery] string? userId) // <-- ADDED
        {
            var query = new GetNotificationsByCorrelationIdQuery
            {
                CorrelationId = correlationId,
                StartDate = startDate,
                EndDate = endDate,
                EventName = eventName,
                SmtpSettingId = smtpSettingId,
                UserId = userId // <-- ADDED
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
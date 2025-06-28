namespace NotificationService.Api.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NotificationService.Application.DTOs;
    using NotificationService.Application.Features.Notifications.Commands.SendNotification;

    [ApiController]
    [Route("v1/[controller]")]
    [Authorize] // <-- SECURE ALL ENDPOINTS IN THIS CONTROLLER
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("/v1/notify")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequestDto notificationRequest)
        {
            var command = new SendNotificationCommand { NotificationRequest = notificationRequest };
            var correlationId = await _mediator.Send(command);

            return Accepted(new { CorrelationId = correlationId });
        }
    }
}

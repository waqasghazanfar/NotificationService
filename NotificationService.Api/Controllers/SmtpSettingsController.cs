namespace NotificationService.Api.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NotificationService.Api.Authentication;
    using NotificationService.Application.Features.SmtpSettings.Commands.CreateSmtpSetting;
    using NotificationService.Application.Features.SmtpSettings.Commands.UpdateSmtpSetting;

    [ApiController]
    [Route("v1/[controller]")]
    [ApiKey] // Apply the API Key authentication to the whole controller
    public class SmtpSettingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SmtpSettingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(Name = "AddSmtpSetting")]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateSmtpSettingCommand createSmtpSettingCommand)
        {
            var id = await _mediator.Send(createSmtpSettingCommand);
            return Ok(id);
        }

        [HttpPut("{id}", Name = "UpdateSmtpSetting")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateSmtpSettingCommand updateSmtpSettingCommand)
        {
            if (id != updateSmtpSettingCommand.Id)
            {
                return BadRequest("ID in URL and body must match.");
            }
            await _mediator.Send(updateSmtpSettingCommand);
            return NoContent();
        }
    }
}
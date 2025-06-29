namespace NotificationService.Api.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NotificationService.Api.Authentication;
    using NotificationService.Application.Features.SmtpSettings.Commands.CreateSmtpSetting;
    using NotificationService.Application.Features.SmtpSettings.Commands.UpdateSmtpSetting;
    using NotificationService.Application.Features.SmtpSettings.Queries;
    using NotificationService.Application.Features.SmtpSettings.Queries.GetSmtpSettingsList;
    using NotificationService.Application.Features.SmtpSettings.Queries.GetTemplatesList;
    using NotificationService.Application.Features.Templates.Commands.CreateTemplate;
    using NotificationService.Application.Features.Templates.Queries.GetTemplatesList;

    [ApiController]
    [Route("api/[controller]")]
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

        [HttpGet("{id}", Name = "GetSmtpSettingsById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SmtpSettingsListVm>> GetSmtpSettingsById(Guid id)
        {
            var smtpSettingsVm = await _mediator.Send(new GetSmtpSettingsByIdQuery { Id = id });

            if (smtpSettingsVm == null)
            {
                return NotFound();
            }

            return Ok(smtpSettingsVm);
        }

        [HttpGet(Name = "SmtpSettings")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<SmtpSettingsListVm>>> GetAllSmtpSettings()
        {
            var dtos = await _mediator.Send(new GetSmtpSettingsListQuery());
            return Ok(dtos);
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
namespace NotificationService.Api.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NotificationService.Api.Authentication;
    using NotificationService.Application.Features.Templates.Commands.CreateTemplate;
    using NotificationService.Application.Features.Templates.Queries.GetTemplatesList;

    [ApiController]
    [Route("v1/[controller]")]
    [ApiKey] // Apply the API Key authentication to the whole controller
    public class TemplatesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TemplatesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(Name = "GetAllTemplates")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<TemplateListVm>>> GetAllTemplates()
        {
            var dtos = await _mediator.Send(new GetTemplatesListQuery());
            return Ok(dtos);
        }

        [HttpPost(Name = "AddTemplate")]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateTemplateCommand createTemplateCommand)
        {
            var id = await _mediator.Send(createTemplateCommand);
            return Ok(id);
        }
    }
}
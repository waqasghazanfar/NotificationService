namespace NotificationService.Api.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NotificationService.Api.Authentication;
    using NotificationService.Application.Features.Templates.Commands.CreateTemplate;
    using NotificationService.Application.Features.Templates.Queries.GetTemplatesList;

    [ApiController]
    [Route("api/template")]
    [ApiKey] // Apply the API Key authentication to the whole controller
    public class TemplatesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TemplatesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(Name = "Templates")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<TemplateListVm>>> GetAllTemplates()
        {
            var dtos = await _mediator.Send(new GetTemplatesListQuery());
            return Ok(dtos);
        }
        [HttpGet("{id}", Name = "GetTemplateById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TemplateListVm>> GetTemplateById(Guid id)
        {
            var templateVm = await _mediator.Send(new GetTemplatesByIdQuery { Id = id });

            if (templateVm == null)
            {
                return NotFound();
            }

            return Ok(templateVm);
        }
        [HttpPost(Name = "Template")]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateTemplateCommand createTemplateCommand)
        {
            var id = await _mediator.Send(createTemplateCommand);
            return Ok(id);
        }
    }
}
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedEvents;
using System.ComponentModel.DataAnnotations;

namespace Orchestrator.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrchestratorController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrchestratorController(IMediator mediator, IPublishEndpoint publishEndpoint)
        {
            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        [Route("{guestEmail}/{grade}/{hostEmail}")]
        public async Task<ActionResult<string>> GradeHostAsync([FromRoute(Name = "guestEmail"), Required] string guestEmail, [FromRoute(Name = "grade"), Required] int grade, [FromRoute(Name = "hostEmail"), Required] string hostEmail)
        {
            //TODO: send via masstransit message to Grading service
            var @event = new CreateHostGradeEvent()
            {
                Date = DateTime.Now,
                Grade = grade,
                GuestEmail = guestEmail,
                HostEmail = hostEmail
            };
            await _publishEndpoint.Publish(@event);
            return Ok("Transaction started");
        }
    }
}

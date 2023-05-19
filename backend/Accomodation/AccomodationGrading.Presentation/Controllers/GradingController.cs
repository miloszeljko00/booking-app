using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using AccomodationGradingDomain.Entities;
using AccomodationGradingApplication.Dtos;
using AccomodationGradingApplication.Grading.Commands;
using Notification.Application.Notification.Queries;

namespace AccomodationGrading.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GradingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GradingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("host")]
        public async Task<ActionResult<HostGrading>> CreateHostGrading([FromBody] CreateHostGradingDTO createHostGradingDTO)
        {
            var command = new CreateHostGradingCommand(
               createHostGradingDTO
               );

            var result = await _mediator.Send(command);
            return Created("Successfully graded host", result);
        }

        [HttpPut]
        [Route("host")]
        public async Task<ActionResult<HostGrading>> UpdateHostGrading([FromBody] UpdateHostGradingDTO updateHostGradingDTO)
        {
            var command = new UpdateHostGradingCommand(
               updateHostGradingDTO
               );

            try
            {
                var result = await _mediator.Send(command);
                return Created("Successfully updated grade for host", result);
            }
            catch(Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("host")]
        public async Task<ActionResult<List<HostGradingDTO>>> GetHostGrading()
        {
            var command = new GetHostGradingQuery();

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete]
        [Route("host/{gradeId}")]
        public async Task<ActionResult<HostGrading>> DeleteHostGrading([FromRoute(Name = "gradeId"), Required] Guid gradeId)
        {
            var command = new DeleteHostGradingCommand(gradeId);

            try
            {
                var result = await _mediator.Send(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}

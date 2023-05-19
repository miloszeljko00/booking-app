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
        public async Task<ActionResult<HostGrading>> CreateHostGrading([FromBody] CreateHostGradingDTO createHostGradingDTO)
        {
            var command = new CreateHostGradingCommand(
               createHostGradingDTO
               );

            var result = await _mediator.Send(command);
            return Created("Successfully graded host", result);
        }

    }
}

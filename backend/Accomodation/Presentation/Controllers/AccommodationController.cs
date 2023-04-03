using Application.Accommodation.Commands;
using Application.Accommodation.Queries;
using Application.AccommodationOfferFolder.Commands;
using Application.AccommodationOfferFolder.Queries;
using Application.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccommodationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccommodationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<Accommodation>>> GetAllAccommodations()
        {
            var query = new GetAllAccommodationsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Accommodation>> CreateAccommodation([FromBody] AccommodationDTO createAccommodationDto)
        {
            var command = new CreateAccommodationCommand(
                createAccommodationDto
                );

            var result = await _mediator.Send(command);

            return Created("neki uri", result);
        }
    }
}

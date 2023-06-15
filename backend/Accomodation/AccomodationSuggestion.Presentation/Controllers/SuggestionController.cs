using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Diagnostics;
using AccomodationSuggestion.Application.Dtos;
using AccomodationSuggestion.Application.Suggestion.Queries;
using AccomodationSuggestion.Domain.Entities;

namespace AccomodationSuggestion.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SuggestionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SuggestionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("get-suggested-flights")]
        public async Task<ActionResult<GetAllSuggestedFlightsResponse>> GetAllSuggestedFlights([FromBody] GetAllSugestedFlightsDTO getAllSugestedFlightsDTO)
        {
            var query = new GetAllSuggestedFlightsQuery(getAllSugestedFlightsDTO);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpPost]
        [Route("book-flight")]
        public async Task<ActionResult<bool>> BookFlight([FromBody] BookFlightDto bookFlightDto)
        {
            var query = new BookFlightQuery(bookFlightDto);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult<UserNode>> getFirstUserNode()
        {
            var query = new GetAllUserNodesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        [Route("recommend-accomodation/{guestEmail}")]
        public async Task<ActionResult<List<AccommodationNode>>> getRecommendedAccommodation(string guestEmail)
        {
            var query = new GetRecommendedAccommodationQuery(guestEmail);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}

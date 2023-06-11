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
        public async Task<ActionResult<List<GetAllSugestedFlightsDTO>>> GetAllAccommodations([FromBody] GetAllSugestedFlightsDTO getAllSugestedFlightsDTO)
        {
            var query = new GetAllSuggestedFlightsQuery(getAllSugestedFlightsDTO);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}

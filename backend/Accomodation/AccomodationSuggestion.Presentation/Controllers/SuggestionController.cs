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
using OpenTracing;
using Prometheus;

namespace AccomodationSuggestion.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SuggestionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITracer _tracer;

        Counter SuggestionCounter = Metrics.CreateCounter("suggestion_counter", "Number of requests for Suggestion microservice" +
            "for given endpoint and with given status code", new CounterConfiguration
            {
                LabelNames = new[] { "microservice", "endpoint", "status" }
            });

        public SuggestionController(IMediator mediator, ITracer tracer)
        {
            _tracer = tracer;
            _mediator = mediator;
        }

        [HttpPost]
        [Route("get-suggested-flights")]
        public async Task<ActionResult<GetAllSuggestedFlightsResponse>> GetAllSuggestedFlights([FromBody] GetAllSugestedFlightsDTO getAllSugestedFlightsDTO)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("Get all suggested flights");
            if (!ModelState.IsValid)
            {
                SuggestionCounter.WithLabels("suggestion", "get_all_suggested_flights", "400").Inc();
                return BadRequest("Invalid request");
            }
            var query = new GetAllSuggestedFlightsQuery(getAllSugestedFlightsDTO);
            var result = await _mediator.Send(query);
            SuggestionCounter.WithLabels("suggestion", "get_all_suggested_flights", "200").Inc();
            return Ok(result);
        }
        [HttpPost]
        [Route("book-flight")]
        public async Task<ActionResult<bool>> BookFlight([FromBody] BookFlightDto bookFlightDto)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("Book chosen flight");
            if (!ModelState.IsValid)
            {
                SuggestionCounter.WithLabels("suggestion", "book_flight", "400").Inc();
                return BadRequest("Invalid request");
            }
            var query = new BookFlightQuery(bookFlightDto);
            var result = await _mediator.Send(query);
            SuggestionCounter.WithLabels("suggestion", "book_flight", "200").Inc();
            return Ok(result);
        }
    }
}

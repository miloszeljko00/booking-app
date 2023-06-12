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
using AccomodationGradingApplication.Grading.Queries;
using OpenTracing;
using Prometheus;

namespace AccomodationGrading.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GradingController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITracer _tracer;

        Counter GradingCounter = Metrics.CreateCounter("grading_counter", "Number of requests for Grading microservice" +
            "for given endpoint and with given status code", new CounterConfiguration
            {
                LabelNames = new[] { "microservice", "endpoint", "status" }
            });

        public GradingController(IMediator mediator, ITracer tracer)
        {
            _tracer = tracer;
            _mediator = mediator;
        }

        [HttpPost]
        [Route("host")]
        public async Task<ActionResult<HostGrading>> CreateHostGrading([FromBody] CreateHostGradingDTO createHostGradingDTO)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("Create grade for host");
            if (!ModelState.IsValid)
            {
                GradingCounter.WithLabels("grading", "create_host_grading", "400").Inc();
                return BadRequest("Invalid request");
            }
            var command = new CreateHostGradingCommand(
               createHostGradingDTO
               );

            var result = await _mediator.Send(command);
            GradingCounter.WithLabels("grading", "create_host_grading", "201").Inc();
            return Created("Successfully graded host", result);
        }

        [HttpPost]
        [Route("accommodation")]
        public async Task<ActionResult<AccommodationGrading>> CreateAccommodationGrading([FromBody] CreateAccommodationGradingDTO createAccommodationGradingDTO)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("Create grade for accommodation");
            if (!ModelState.IsValid)
            {
                GradingCounter.WithLabels("grading", "create_accommodation_grading", "400").Inc();
                return BadRequest("Invalid request");
            }
            var command = new CreateAccommodationGradingCommand(
               createAccommodationGradingDTO
               );

            var result = await _mediator.Send(command);
            GradingCounter.WithLabels("grading", "create_accommodation_grading", "201").Inc();
            return Created("Successfully graded accommodation", result);
        }

        [HttpPut]
        [Route("host")]
        public async Task<ActionResult<HostGrading>> UpdateHostGrading([FromBody] UpdateHostGradingDTO updateHostGradingDTO)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("Update grade for host");
            if (!ModelState.IsValid)
            {
                GradingCounter.WithLabels("grading", "update_host_grading", "400").Inc();
                return BadRequest("Invalid request");
            }
            var command = new UpdateHostGradingCommand(
               updateHostGradingDTO
               );

            try
            {
                var result = await _mediator.Send(command);
                GradingCounter.WithLabels("grading", "update_host_grading", "201").Inc();
                return Created("Successfully updated grade for host", result);
            }
            catch(Exception ex) {
                GradingCounter.WithLabels("grading", "update_host_grading", "400").Inc();
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("accommodation")]
        public async Task<ActionResult<AccommodationGrading>> UpdateAccommodationGrading([FromBody] UpdateAccommodationGradingDTO updateAccommodationGradingDTO)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("Update grade for accommodation");
            if (!ModelState.IsValid)
            {
                GradingCounter.WithLabels("grading", "update_accommodation_grading", "400").Inc();
                return BadRequest("Invalid request");
            }
            var command = new UpdateAccommodationGradingCommand(
               updateAccommodationGradingDTO
               );

            try
            {
                var result = await _mediator.Send(command);
                GradingCounter.WithLabels("grading", "update_accommodation_grading", "201").Inc();
                return Created("Successfully updated grade for accommodation", result);
            }
            catch (Exception ex)
            {
                GradingCounter.WithLabels("grading", "update_accommodation_grading", "400").Inc();
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("host")]
        public async Task<ActionResult<List<HostGradingDTO>>> GetHostGrading()
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("Get grades for host");
            var command = new GetHostGradingQuery();

            var result = await _mediator.Send(command);
            GradingCounter.WithLabels("grading", "get_host_grading", "200").Inc();
            return Ok(result);
        }

        [HttpGet]
        [Route("accommodation")]
        public async Task<ActionResult<List<AccommodationGradingDTO>>> GetAccommodationGrading()
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("Get grades for accommodation");
            var command = new GetAccommodationGradingQuery();

            var result = await _mediator.Send(command);
            GradingCounter.WithLabels("grading", "get_accommodation_grading", "200").Inc();
            return Ok(result);
        }

        [HttpDelete]
        [Route("host/{gradeId}")]
        public async Task<ActionResult<HostGrading>> DeleteHostGrading([FromRoute(Name = "gradeId"), Required] Guid gradeId)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("Delete grade for host");
            var command = new DeleteHostGradingCommand(gradeId);

            try
            {
                var result = await _mediator.Send(command);
                GradingCounter.WithLabels("grading", "delete_host_grading", "204").Inc();
                return NoContent();
            }
            catch (Exception ex)
            {
                GradingCounter.WithLabels("grading", "delete_host_grading", "400").Inc();
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("accommodation/{gradeId}")]
        public async Task<ActionResult<AccommodationGrading>> DeleteAccommodationGrading([FromRoute(Name = "gradeId"), Required] Guid gradeId)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("Delete grade for accommodation");
            var command = new DeleteAccommodationGradingCommand(gradeId);

            try
            {
                var result = await _mediator.Send(command);
                GradingCounter.WithLabels("grading", "delete_accommodation_grading", "204").Inc();
                return NoContent();
            }
            catch (Exception ex)
            {
                GradingCounter.WithLabels("grading", "delete_accommodation_grading", "400").Inc();
                return BadRequest(ex.Message);
            }
        }

    }
}

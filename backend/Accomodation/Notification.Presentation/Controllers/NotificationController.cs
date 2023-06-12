using MediatR;
using Microsoft.AspNetCore.Mvc;
using Notification.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notification.Application.Notification.Commands;
using Notification.Application.Notification.Queries;
using Notification.Application.Dtos;
using MongoDB.Driver;
using OpenTracing;
using Prometheus;

namespace Notification.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITracer _tracer;

        Counter NotificationCounter = Metrics.CreateCounter("notification_counter", "Number of requests for Notification microservice" +
            "for given endpoint and with given status code", new CounterConfiguration
            {
                LabelNames = new[] { "microservice", "endpoint", "status" }
            });

        public NotificationController(IMediator mediator, ITracer tracer)
        {
            _tracer = tracer;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{guestEmail}/guest-notifications")]
        public async Task<ActionResult<GuestNotificationDTO>> GetNotificationsByGuest([FromRoute(Name = "guestEmail"), Required] string guestEmail)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("Get all notification options by guest");
            var query = new GetNotificationsByGuestQuery(guestEmail);
            try
            {
                var result = await _mediator.Send(query);
                NotificationCounter.WithLabels("notification", "get_notifications_by_guest", "200").Inc();
                return Ok(result);
            }
            catch (Exception e)
            {
                NotificationCounter.WithLabels("notification", "get_notifications_by_guest", "400").Inc();
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [Route("guest-notifications")]
        public async Task<ActionResult<GuestNotification>> SetGuestNotification([FromBody] CreateGuestNotificationDTO createGuestNotificationDTO)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("Set notification options for guest");
            if (!ModelState.IsValid)
            {
                NotificationCounter.WithLabels("notification", "set_guest_notification", "400").Inc();
                return BadRequest("Invalid request");
            }
            var query = new SetGuestNotificationCommand(createGuestNotificationDTO);
            var result = await _mediator.Send(query);
            NotificationCounter.WithLabels("notification", "set_guest_notification", "200").Inc();
            return Ok(result);
        }

        [HttpGet]
        [Route("{hostEmail}/host-notifications")]
        public async Task<ActionResult<HostNotificationDTO>> GetNotificationsByHost([FromRoute(Name = "hostEmail"), Required] string hostEmail)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("Get all notification options by host");
            var query = new GetNotificationsByHostQuery(hostEmail);
            try
            {
                var result = await _mediator.Send(query);
                NotificationCounter.WithLabels("notification", "get_notifications_by_host", "200").Inc();
                return Ok(result);
            }
            catch (Exception e)
            {
                NotificationCounter.WithLabels("notification", "get_notifications_by_host", "400").Inc();
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [Route("host-notifications")]

        public async Task<ActionResult<HostNotification>> SetHostNotification([FromBody] CreateHostNotificationDTO createHostNotificationDTO)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("Set notification options for host");
            if (!ModelState.IsValid)
            {
                NotificationCounter.WithLabels("notification", "set_host_notification", "400").Inc();
                return BadRequest("Invalid request");
            }
            var query = new SetHostNotificationCommand(createHostNotificationDTO);
            var result = await _mediator.Send(query);
            NotificationCounter.WithLabels("notification", "set_host_notification", "200").Inc();
            return Ok(result);
        }
    }
}

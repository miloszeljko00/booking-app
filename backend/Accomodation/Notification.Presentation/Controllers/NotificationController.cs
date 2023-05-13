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

namespace Notification.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{guestEmail}/guest-notifications")]
        public async Task<ActionResult<GuestNotificationDTO>> GetNotificationsByGuest([FromRoute(Name = "guestEmail"), Required] string guestEmail)
        {
            var query = new GetNotificationsByGuestQuery(guestEmail);
            try
            {
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<GuestNotification>> SetGuestNotification([FromBody] CreateGuestNotificationDTO createGuestNotificationDTO)
        {
            var query = new SetGuestNotificationCommand(createGuestNotificationDTO);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

    }
}

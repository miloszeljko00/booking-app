using MediatR;
using Microsoft.AspNetCore.Mvc;
using Notification.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<ActionResult<List<GuestNotification>>> GetNotificationsByGuest([FromRoute(Name = "guestEmail"), Required] string guestEmail)
        {
            //var query = new GetNotificationsByGuestQuery(guestEmail);
            //var result = await _mediator.Send(query);

            return Ok(/*result.ToList()*/);
        }



    }
}

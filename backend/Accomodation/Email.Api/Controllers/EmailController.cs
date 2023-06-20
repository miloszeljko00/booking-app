using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Email.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly ILogger<EmailController> _logger;

        public EmailController(ILogger<EmailController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("{guestEmail}/{grade}")]
        public ActionResult<bool> SendEmail([FromRoute(Name = "guestEmail"), Required] string guestEmail, [FromRoute(Name = "grade"), Required] string grade)
        {
            return Ok(true);
        }
    }
}
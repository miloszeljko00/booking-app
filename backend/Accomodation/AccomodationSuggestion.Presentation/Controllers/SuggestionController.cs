using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

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

    }
}

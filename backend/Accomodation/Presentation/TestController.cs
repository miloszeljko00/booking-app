using Application.AccommodationOfferFolder.Commands;
using Application.AccommodationOfferFolder.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
//NEMOJTE BRISATI OVAJ KONTROLER
namespace Presentation
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        public static CancellationTokenSource Cts = new CancellationTokenSource();
        private readonly IMediator _mediator;

        public TestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<AccommodationOffer>>> GetAllAccommodationOffers()
        {
            // Send the GetAllAccommodationOffersQuery to the MediatR pipeline
            var query = new GetAllAccommodationOffersQuery();
            try
            {
                var result = await _mediator.Send(query, Cts.Token);

                return Ok(result);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Zahtev je otkazan!");
                return BadRequest("Zahtev je otkazan!");
            }
        }

        [HttpPost]
        public async Task<ActionResult<AccommodationOffer>> CreateAccommodationOffer()
        {
            var command = new CreateAccommodationOfferCommand("Velja testira kreiranje", DateTime.UtcNow, DateTime.UtcNow.AddDays(3));
            
            var result = await _mediator.Send(command);

            return Ok(result);
        }
        [HttpGet("stop")]
        public void StopGetingAllAccommodationOffers()
        {
            Cts.Cancel();
        }
    }
}
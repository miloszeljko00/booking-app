using FlightsBooking.Services;
using FlightsBookingAPI.Controllers;
using FlightsBookingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace FlightsBookingAPI.ControllersImpl
{
    public class FlightController : FlightApiController
    {
        private IFlightService flightService;

        public FlightController(IFlightService flightService)
        {
            this.flightService = flightService;
        }

        public override IActionResult DeleteFlightsId([FromRoute(Name = "FlightId"), Required] Guid flightId)
        {
            throw new NotImplementedException();
        }

        public override IActionResult GetFlights()
        {
            return new OkObjectResult(flightService.Test());
        }

        public override IActionResult GetFlightsId([FromRoute(Name = "FlightId"), Required] Guid flightId)
        {
            throw new NotImplementedException();
        }

        public override IActionResult PostFlights([FromBody] FlightCreateRequest flightCreateRequest)
        {
            throw new NotImplementedException();
        }

        public override IActionResult PostFlightsIdActionsBuyTicket([FromRoute(Name = "FlightId"), Required] string flightId, [FromBody] FlightBuyTicketsRequest flightBuyTicketsRequest)
        {
            throw new NotImplementedException();
        }
    }
}

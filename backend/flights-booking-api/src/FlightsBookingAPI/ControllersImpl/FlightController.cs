using FlightsBooking.Services;
using FlightsBookingAPI.Controllers;
using FlightsBookingAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Threading.Tasks;

namespace FlightsBookingAPI.ControllersImpl
{
    public class FlightController : FlightApiController
    {
        private IFlightService flightService;

        public FlightController(IFlightService flightService)
        {
            this.flightService = flightService;
        }

        public override async Task<IActionResult> DeleteFlightsIdAsync([FromRoute(Name = "FlightId"), Required] Guid flightId)
        {
            var flight = await this.flightService.GetAsync(flightId);
            if (flight is null)
            {
                return NotFound();
            }
            flight.IsDeleted = true;
            await this.flightService.UpdateAsync(flightId, flight);

            return NoContent();
        }

        public override async Task<ActionResult<FlightGetAllResponse>> GetFlights()
        {
            List < FlightsBooking.Models.Flight > flights = await this.flightService.GetAsync();
            List<Flight> fs = new List<Flight>();
            foreach (FlightsBooking.Models.Flight flight in flights)
            {
                List<SoldTicket> st = new List<SoldTicket>();
                foreach(FlightsBooking.Models.SoldTicket soldTicket in flight.SoldTickets)
                {
                    st.Add(new SoldTicket
                    {
                        FlightTicketId = new Guid(soldTicket.Id),
                        Purchased = soldTicket.Purchased,
                        Price = soldTicket.Price
                    });
                }
                fs.Add(
                    new Flight
                    {
                        FlightId = flight.Id,
                        Arrival = new Arrival { City = flight.Arrival.City, Time = flight.Arrival.Time },
                        Departure = new Departure { City = flight.Departure.City, Time = flight.Departure.Time },
                        SoldTickets = st,
                        TotalTickets = flight.TotalTickets,
                        AvailableTickets = flight.CalculateNumberOfAvailableTicketForTheFlight(),
                        TicketPrice = flight.TicketPrice,
                        TotalPrice = flight.CalculateTotalPriceForFlight(),
                        Passed = flight.IsFlightPassed(),
                        Canceled = flight.IsDeleted
                    }) ;
            };
            return new FlightGetAllResponse { Flights = fs };
        }

        public override async Task<ActionResult<FlightGetResponse>> GetFlightsId([FromRoute(Name = "FlightId"), Required] Guid flightId)
        {
            FlightsBooking.Models.Flight flight = await this.flightService.GetAsync(flightId);
            if (flight is null)
            {
                return NotFound();
            }
            List<SoldTicket> st = new List<SoldTicket>();
            foreach (FlightsBooking.Models.SoldTicket soldTicket in flight.SoldTickets)
            {
                st.Add(new SoldTicket
                {
                    FlightTicketId = new Guid(soldTicket.Id),
                    Purchased = soldTicket.Purchased,
                    Price = soldTicket.Price
                });
            }
            Flight f = new Flight
            {
                FlightId = flight.Id,
                Arrival = new Arrival { City = flight.Arrival.City, Time = flight.Arrival.Time },
                Departure = new Departure { City = flight.Departure.City, Time = flight.Departure.Time },
                SoldTickets = st,
                TotalTickets = flight.TotalTickets,
                AvailableTickets = flight.CalculateNumberOfAvailableTicketForTheFlight(),
                TicketPrice = flight.TicketPrice,
                TotalPrice = flight.CalculateTotalPriceForFlight(),
                Passed = flight.IsFlightPassed(),
                Canceled = flight.IsDeleted
            } ;
            return new FlightGetResponse { Flight = f};
        }

        public override async Task<ActionResult<FlightCreateRequest>> PostFlights([FromBody] FlightCreateRequest flightCreateRequest)
        {
            FlightsBooking.Models.Flight flight = new FlightsBooking.Models.Flight(
                new FlightsBooking.Models.Departure(flightCreateRequest.Departure.Time, flightCreateRequest.Departure.City),
                new FlightsBooking.Models.Arrival(flightCreateRequest.Arrival.Time, flightCreateRequest.Arrival.City),
                flightCreateRequest.TotalTickets,
                flightCreateRequest.TicketPrice, 
                new List<FlightsBooking.Models.SoldTicket>());
            await this.flightService.CreateAsync(flight);
            return flightCreateRequest;
        }

        public override IActionResult PostFlightsIdActionsBuyTicket([FromRoute(Name = "FlightId"), Required] Guid flightId, [FromBody] FlightBuyTicketsRequest flightBuyTicketsRequest)
        {
            throw new NotImplementedException();
        }
    }
}

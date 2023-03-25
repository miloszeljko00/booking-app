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

        public override async Task<IActionResult> DeleteFlightsId([FromRoute(Name = "FlightId"), Required] Guid flightId)
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

        public override async Task<IActionResult> GetFlights()
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
            return Ok(new FlightGetAllResponse { Flights = fs });
        }

        public override async Task<IActionResult> GetFlightsActionsSearch([FromQuery(Name = "arrivalPlace")] string arrivalPlace, [FromQuery(Name = "departurePlace")] string departurePlace, [FromQuery(Name = "departureDate")] string departureDate, [FromQuery(Name = "availableTickets")] decimal? availableTickets)
        {
            List<FlightsBooking.Models.Flight> flights = await this.flightService.SearchAsync(arrivalPlace, departurePlace, departureDate, availableTickets);
            List<Flight> fs = new List<Flight>();
            foreach (FlightsBooking.Models.Flight flight in flights)
            {
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
                    });
            };
            return Ok(new FlightGetAllResponse { Flights = fs });
        }

        public override async Task<IActionResult> GetFlightsId([FromRoute(Name = "FlightId"), Required] Guid flightId)
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
            return Ok(new FlightGetResponse { Flight = f});
        }

        public override async Task<IActionResult> PostFlights([FromBody] FlightCreateRequest flightCreateRequest)
        {
            FlightsBooking.Models.Flight flight = new FlightsBooking.Models.Flight(
                new FlightsBooking.Models.Departure(flightCreateRequest.Departure.Time, flightCreateRequest.Departure.City),
                new FlightsBooking.Models.Arrival(flightCreateRequest.Arrival.Time, flightCreateRequest.Arrival.City),
                flightCreateRequest.TotalTickets,
                flightCreateRequest.TicketPrice, 
                new List<FlightsBooking.Models.SoldTicket>());
            await this.flightService.CreateAsync(flight);
            return Ok(flightCreateRequest);
        }

       
        public override Task<IActionResult> PostFlightsIdActionsBuyTicket([FromRoute(Name = "FlightId"), Required] string flightId, [FromBody] FlightBuyTicketsRequest flightBuyTicketsRequest)
        {
            throw new NotImplementedException();
        }

      
    }
}

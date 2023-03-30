using FlightsBooking.Services;
using FlightsBookingAPI.Controllers;
using FlightsBookingAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FlightsBookingAPI.ControllersImpl
{
    public class UserController : UserApiController
    {
        private IFlightService flightService;

        public UserController(IFlightService flightService)
        {
            this.flightService = flightService;
        }
        [Authorize(Roles="user")]
        public override async Task<IActionResult> GetUsersIdFlightTickets([FromRoute(Name = "UserId"), Required] string userId)
        {
            //throw new System.NotImplementedException();
            List<FlightsBooking.Models.Flight> flights = await this.flightService.getUserFlights(userId);
            List<UserFlightTicket> userTickets = new List<UserFlightTicket>();
            foreach(var flight in flights)
            {
                foreach (var soldTicket in flight.SoldTickets)
                {
                    if (soldTicket.UserId.Equals(userId))
                    {
                        UserFlight uf =
                            new UserFlight
                            {
                                FlightId = flight.Id,
                                Departure = new Departure { City = flight.Departure.City, Time = flight.Departure.Time },
                                Arrival = new Arrival { City = flight.Arrival.City, Time = flight.Arrival.Time },
                                Passed = flight.IsFlightPassed(),
                                Canceled = flight.IsDeleted
                            };
                        UserFlightTicket uft =
                            new UserFlightTicket
                            {
                                FlightTicketId = soldTicket.Id,
                                Purchased = soldTicket.Purchased,
                                Price = soldTicket.Price,
                                Flight = uf

                            };
                        userTickets.Add(uft);
                    }
                }

            }
            return Ok(new UserFlightTicketGetAllResponse { FlightTickets = userTickets });
        }

        public override Task<IActionResult> GetUsersUserIdFlightTicketsFlightTicketId([FromRoute(Name = "UserId"), Required] string userId, [FromRoute(Name = "FlightTicketId"), Required] string flightTicketId)
        {
            throw new System.NotImplementedException();
        }
    }
}

using FlightsBooking.Models;
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
        private IUserService userService;

        public UserController(IFlightService flightService, IUserService userService)
        {
            this.flightService = flightService;
            this.userService = userService;
        }

        [Authorize(Roles = "user")]
        [HttpGet]
        [Route("/users/getApiKey/{userId}")]
        public async Task<IActionResult> GetApiKey(string userId)
        {
            var user = await userService.GetAsync(Guid.Parse(userId));
            if(user == null) return Ok(new User() { Id=Guid.Parse(userId), ApiKey = "" });
            return Ok(new User() { Id = user.Id, ApiKey = user.ApiKey });
        }

        [Authorize(Roles = "user")]
        [HttpGet]
        [Route("/users/generateApiKey/{userId}")]
        public async Task<IActionResult> GenerateApiKey(string userId)
        {
            var apiKey = Guid.NewGuid();
            var user = await userService.GetAsync(Guid.Parse(userId));
            if (user == null)
            {
                user = new User() { Id = Guid.Parse(userId), ApiKey = apiKey.ToString() };
                await userService.CreateAsync(user);
            }
            else
            {
                user.ApiKey = apiKey.ToString();
                await userService.UpdateAsync(user.Id, user);
            }
            return Ok(user);
        }
        [Authorize(Roles = "user")]
        [HttpDelete]
        [Route("/users/revokeApiKey/{userId}")]
        public async Task<IActionResult> RevokeApiKey(string userId)
        {
            var user = await userService.GetAsync(Guid.Parse(userId));
            if (user == null) return BadRequest();
            await userService.RemoveAsync(user.Id);
            return Ok();
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
                                Departure = new Models.Departure { City = flight.Departure.City, Time = flight.Departure.Time },
                                Arrival = new Models.Arrival { City = flight.Arrival.City, Time = flight.Arrival.Time },
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

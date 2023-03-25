/*
 * FlightBookingAPI
 *
 * API aplikacije za kupovinu avionskih karata
 *
 * The version of the OpenAPI document: 1.0
 * 
 * Generated by: https://openapi-generator.tech
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json;
using FlightsBookingAPI.Attributes;
using FlightsBookingAPI.Models;

namespace FlightsBookingAPI.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public abstract class UserApiController : ControllerBase
    { 
        /// <summary>
        /// GET FlightTickets
        /// </summary>
        /// <remarks>Endpoint koji vraca podatke o svim kartama koje je odabrani korisnik kupio.</remarks>
        /// <param name="userId">Identifikator korisnika.</param>
        /// <param name="body"></param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request: UserId does not exist.</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [Route("/users/{UserId}/flight-tickets")]
        [Consumes("application/json")]
        [ValidateModelState]
        [SwaggerOperation("GetUsersIdFlightTickets")]
        [SwaggerResponse(statusCode: 200, type: typeof(UserFlightTicketGetAllResponse), description: "OK")]
        public abstract IActionResult GetUsersIdFlightTickets([FromRoute (Name = "UserId")][Required]string userId, [FromBody]Object body);

        /// <summary>
        /// GET FlightTicket
        /// </summary>
        /// <remarks>Endpoint za dobavljanje podataka o odabranoj karti za let odabranog korisnika.</remarks>
        /// <param name="userId">Identifikator korisnika</param>
        /// <param name="flightTicketId">Identifikator karte leta</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request: UserId does not exist.</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found: Flight ticket with FlightTicketId not found.</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [Route("/users/{UserId}/flight-tickets/{FlightTicketId}")]
        [ValidateModelState]
        [SwaggerOperation("GetUsersUserIdFlightTicketsFlightTicketId")]
        [SwaggerResponse(statusCode: 200, type: typeof(UserFlightTicketGetResponse), description: "OK")]
        public abstract IActionResult GetUsersUserIdFlightTicketsFlightTicketId([FromRoute (Name = "UserId")][Required]string userId, [FromRoute (Name = "FlightTicketId")][Required]string flightTicketId);
    }
}

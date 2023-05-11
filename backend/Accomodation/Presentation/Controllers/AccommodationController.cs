using Accomodation.Application.Dtos;
using Accomodation.Domain.Primitives.Enums;
using AccomodationApplication.Accommodation.Commands;
using AccomodationApplication.Accommodation.Queries;
using AccomodationApplication.Dtos;
using AccomodationDomain.Entities;
using AccomodationDomain.Primitives.Enums;
using AccomodationDomain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationPresentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccommodationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccommodationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<AccommodationGetAllDTO>>> GetAllAccommodations()
        {
            var query = new GetAllAccommodationsQuery();
            var result = await _mediator.Send(query);
            
            List<AccommodationGetAllDTO> resultList = new List<AccommodationGetAllDTO>();
            foreach(Accommodation acc in result)
            {
                Price? p = acc.GetPriceForSpecificDate(DateTime.Now);
                if (p == null)
                    continue;
                string address = acc.GetAddressAsString();
                string benefits = acc.GetBenefitsAsString();
                AccommodationGetAllDTO dto = new AccommodationGetAllDTO { Name = acc.Name, Address = address, Min = acc.Capacity.Min, Max = acc.Capacity.Max, Price = p.Value, PriceCalculation = acc.PriceCalculation.ToString(), Benefits = benefits, Id = acc.Id.ToString() };
                
     
                resultList.Add(dto);
            }
            return Ok(resultList);
        }

        [HttpPut]
        [Route("reservation")]
        public async Task<ActionResult<ReservationRequest>> CreateReservationRequest([FromBody] ReservationRequestDTO reservationRequestDTO)
        {
            var command = new CreateReservationRequestCommand(
               reservationRequestDTO
               );

            try
            {
                var result = await _mediator.Send(command);
                return Created("Successful reservation", result);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("request")]
        public async Task<ActionResult<Accommodation>> CancelReservationRequest([FromBody] ReservationCancellationDTO reservationRequestCancellationDTO)
        {
            var command = new CancelReservationRequestCommand(
               reservationRequestCancellationDTO
               );

            try
            {
                var result = await _mediator.Send(command);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("reservation")]
        public async Task<ActionResult<Accommodation>> CancelReservation([FromBody] ReservationCancellationDTO reservationCancellationDTO)
        {
            var command = new CancelReservationCommand(
               reservationCancellationDTO
               );

            try
            {
                var result = await _mediator.Send(command);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("{guestEmail}/reservations")]
        public async Task<ActionResult<List<ReservationByGuestDTO>>> GetReservationsByGuest([FromRoute(Name = "guestEmail"), Required] string guestEmail)
        {
            var query = new GetAllReservationsByGuestQuery(guestEmail);
            var result = await _mediator.Send(query);

            return Ok(result.ToList());
        }

        [HttpGet]
        [Route("{guestEmail}/requests")]
        public async Task<ActionResult<List<ReservationRequestByGuestDTO>>> GetRequestsByGuest([FromRoute(Name = "guestEmail"), Required] string guestEmail)
        {
            var query = new GetAllRequestsByGuestQuery(guestEmail);
            var result = await _mediator.Send(query);

            return Ok(result.ToList());
        }

        [HttpGet]
        [Route("{adminEmail}/admin-requests")]
        public async Task<ActionResult<List<ReservationRequestByAdminDTO>>> GetRequestsByAdmin([FromRoute(Name = "adminEmail"), Required] string adminEmail)
        {
            var query = new GetAllRequestsByAdminQuery(adminEmail);
            var result = await _mediator.Send(query);

            return Ok(result.ToList());
        }

        [HttpPost]
        public async Task<ActionResult<Accommodation>> CreateAccommodation([FromBody] AccommodationDTO createAccommodationDto)
        {
            var command = new CreateAccommodationCommand(
                createAccommodationDto
                );

            var result = await _mediator.Send(command);

            return Created("neki uri", result);
        }

        [HttpGet]
        [Route("benefits")]
        public ActionResult<Benefit> GetBenefits()
        {
            List<string> benefitList = new List<string>();

            foreach (Benefit benefit in Enum.GetValues(typeof(Benefit)))
            {
                benefitList.Add(benefit.ToString());
            }
            
            return Ok(benefitList);
        }

    }
}

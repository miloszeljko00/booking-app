using Accomodation.Application.Accommodation.Commands;
using Accomodation.Application.Accommodation.Queries;
using Accomodation.Application.Dtos;
using AccomodationApplication.Accommodation.Commands;
using AccomodationApplication.Accommodation.Queries;
using AccomodationApplication.Dtos;
using AccomodationSuggestionDomain.Entities;
using AccomodationSuggestionDomain.Primitives.Enums;
using AccomodationSuggestionDomain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;
using Prometheus;
using System.ComponentModel.DataAnnotations;

namespace AccomodationPresentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccommodationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITracer _tracer;

        Counter AccommodationCounter = Metrics.CreateCounter("accommodation_counter", "Number of requests for Accommodation microservice" +
            "for given endpoint and with given status code", new CounterConfiguration
        {
            LabelNames = new[] {"microservice", "endpoint", "status"}
        });

        public AccommodationController(IMediator mediator, ITracer tracer)
        {
            _tracer = tracer;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<AccommodationGetAllDTO>>> GetAllAccommodations()
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("Get all accommodation");
            Console.SetOut(Console.Out);
            Console.WriteLine("METHOD: GET, DESCRIPTION: Get all accommodation, TIME: " + DateTime.Now.ToString("dd.MM.yyyy. HH:mm:ss"));

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
                AccommodationGetAllDTO dto = new AccommodationGetAllDTO { Name = acc.Name, Address = address, Min = acc.Capacity.Min, Max = acc.Capacity.Max, Price = p.Value, PriceCalculation = acc.PriceCalculation.ToString(), Benefits = benefits, Id = acc.Id.ToString(), Pictures = acc.Pictures };
                
     
                resultList.Add(dto);
            }
            AccommodationCounter.WithLabels("accommodation", "get_all_accommodation", "200").Inc();
            return Ok(resultList);
        }

        [HttpPut]
        [Route("reservation")]
        public async Task<ActionResult<ReservationRequest>> CreateReservationRequest([FromBody] ReservationRequestDTO reservationRequestDTO)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("Create reservation request");
            Console.SetOut(Console.Out);
            Console.WriteLine("METHOD: PUT, DESCRIPTION: Create reservation request, TIME: " + DateTime.Now.ToString("dd.MM.yyyy. HH:mm:ss"));

            if (!ModelState.IsValid)
            {
                AccommodationCounter.WithLabels("accommodation", "create_reservation_request", "400").Inc();
                return BadRequest("Invalid request");
            }
            var command = new CreateReservationRequestCommand(
               reservationRequestDTO
               );

            try
            {
                var result = await _mediator.Send(command);
                AccommodationCounter.WithLabels("accommodation", "create_reservation_request", "201").Inc();
                return Created("Successful reservation", result);
            }
            catch(Exception e)
            {
                AccommodationCounter.WithLabels("accommodation", "create_reservation_request", "400").Inc();
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [Route("manage-request")]
        public async Task<ActionResult<Accommodation>> ManageReservationRequest([FromBody] ReservationManagementDTO reservationRequestManagementDTO)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("Reservation request review");
            Console.SetOut(Console.Out);
            Console.WriteLine("METHOD: PUT, DESCRIPTION: Reservation request review, TIME: " + DateTime.Now.ToString("dd.MM.yyyy. HH:mm:ss"));

            if (!ModelState.IsValid)
            {
                AccommodationCounter.WithLabels("accommodation", "manage_reservation_request", "400").Inc();
                return BadRequest("Invalid request");
            }
            var command = new ManageReservationRequestCommand(
               reservationRequestManagementDTO
               );

            try
            {
                var result = await _mediator.Send(command);
                AccommodationCounter.WithLabels("accommodation", "manage_reservation_request", "200").Inc();
                return Ok(result);
            }
            catch (Exception e)
            {
                AccommodationCounter.WithLabels("accommodation", "manage_reservation_request", "400").Inc();
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("request")]
        public async Task<ActionResult<Accommodation>> CancelReservationRequest([FromBody] ReservationCancellationDTO reservationRequestCancellationDTO)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("Cancel reservation request");
            Console.SetOut(Console.Out);
            Console.WriteLine("METHOD: DELETE, DESCRIPTION: Cancel reservation request, TIME: " + DateTime.Now.ToString("dd.MM.yyyy. HH:mm:ss"));

            if (!ModelState.IsValid)
            {
                AccommodationCounter.WithLabels("accommodation", "cancel_reservation_request", "400").Inc();
                return BadRequest("Invalid request");
            }
            var command = new CancelReservationRequestCommand(
               reservationRequestCancellationDTO
               );

            try
            {
                var result = await _mediator.Send(command);
                AccommodationCounter.WithLabels("accommodation", "cancel_reservation_request", "204").Inc();
                return NoContent();
            }
            catch (Exception e)
            {
                AccommodationCounter.WithLabels("accommodation", "cancel_reservation_request", "400").Inc();
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("reservation")]
        public async Task<ActionResult<Accommodation>> CancelReservation([FromBody] ReservationCancellationDTO reservationCancellationDTO)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("Cancel reservation");
            Console.SetOut(Console.Out);
            Console.WriteLine("METHOD: DELETE, DESCRIPTION: Cancel reservation, TIME: " + DateTime.Now.ToString("dd.MM.yyyy. HH:mm:ss"));

            if (!ModelState.IsValid)
            {
                AccommodationCounter.WithLabels("accommodation", "cancel_reservation", "400").Inc();
                return BadRequest("Invalid request");
            }
            var command = new CancelReservationCommand(
               reservationCancellationDTO
               );

            try
            {
                var result = await _mediator.Send(command);
                AccommodationCounter.WithLabels("accommodation", "cancel_reservation", "204").Inc();
                return NoContent();
            }
            catch (Exception e)
            {
                AccommodationCounter.WithLabels("accommodation", "cancel_reservation", "400").Inc();
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("{guestEmail}/reservations")]
        public async Task<ActionResult<List<ReservationByGuestDTO>>> GetReservationsByGuest([FromRoute(Name = "guestEmail"), Required] string guestEmail)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("Get all reservations by guest");
            Console.SetOut(Console.Out);
            Console.WriteLine("METHOD: GET, DESCRIPTION: Get all reservations by guest, TIME: " + DateTime.Now.ToString("dd.MM.yyyy. HH:mm:ss"));

            var query = new GetAllReservationsByGuestQuery(guestEmail);
            var result = await _mediator.Send(query);
            AccommodationCounter.WithLabels("accommodation", "get_reservations_by_guest", "200").Inc();
            return Ok(result.ToList());
        }

        [HttpGet]
        [Route("{hostEmail}/highlighted-host")]
        public async Task<ActionResult<bool>> CheckHighlightedHost([FromRoute(Name = "hostEmail"), Required] string hostEmail)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("Check if the host is highlighted");
            Console.SetOut(Console.Out);
            Console.WriteLine("METHOD: GET, DESCRIPTION: Check if the host is highlighted, TIME: " + DateTime.Now.ToString("dd.MM.yyyy. HH:mm:ss"));

            var query = new CheckHighlightedHostQuery(hostEmail);
            var result = await _mediator.Send(query);
            AccommodationCounter.WithLabels("accommodation", "check_highlighted_host", "200").Inc();
            return Ok(result);
        }

        [HttpGet]
        [Route("{guestEmail}/requests")]
        public async Task<ActionResult<List<ReservationRequestByGuestDTO>>> GetRequestsByGuest([FromRoute(Name = "guestEmail"), Required] string guestEmail)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("Get all reservation requests by guest");
            Console.SetOut(Console.Out);
            Console.WriteLine("METHOD: GET, DESCRIPTION: Get all reservation requests by guest, TIME: " + DateTime.Now.ToString("dd.MM.yyyy. HH:mm:ss"));

            var query = new GetAllRequestsByGuestQuery(guestEmail);
            var result = await _mediator.Send(query);
            AccommodationCounter.WithLabels("accommodation", "get_requests_by_guest", "200").Inc();
            return Ok(result.ToList());
        }

        [HttpGet]
        [Route("{adminEmail}/admin-requests")]
        public async Task<ActionResult<List<ReservationRequestByAdminDTO>>> GetRequestsByAdmin([FromRoute(Name = "adminEmail"), Required] string adminEmail)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("Get all reservation requests by host");
            Console.SetOut(Console.Out);
            Console.WriteLine("METHOD: GET, DESCRIPTION: Get all reservation requests by host, TIME: " + DateTime.Now.ToString("dd.MM.yyyy. HH:mm:ss"));

            var query = new GetAllRequestsByAdminQuery(adminEmail);
            var result = await _mediator.Send(query);
            AccommodationCounter.WithLabels("accommodation", "get_requests_by_host", "200").Inc();
            return Ok(result.ToList());
        }

        [HttpGet]
        [Route("{guestEmail}/hosts")]
        public async Task<ActionResult<List<string>>> GetHostsByGuestReservations([FromRoute(Name = "guestEmail"), Required] string guestEmail)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("Get all accommodation hosts for guest's reservations");
            Console.SetOut(Console.Out);
            Console.WriteLine("METHOD: GET, DESCRIPTION: Get all accommodation hosts for guest's reservations, TIME: " + DateTime.Now.ToString("dd.MM.yyyy. HH:mm:ss"));

            var query = new GetHostsByGuestReservationsQuery(guestEmail);
            var result = await _mediator.Send(query);
            AccommodationCounter.WithLabels("accommodation", "get_hosts_by_guest_reservations", "200").Inc();
            return Ok(result.ToList());
        }

        [HttpGet]
        [Route("{guestEmail}/accommodation")]
        public async Task<ActionResult<List<AccommodationMainDTO>>> GetAccommodationByGuestReservations([FromRoute(Name = "guestEmail"), Required] string guestEmail)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("Get all accommodation for guest's reservations");
            Console.SetOut(Console.Out);
            Console.WriteLine("METHOD: GET, DESCRIPTION: Get all accommodation for guest's reservations, TIME: " + DateTime.Now.ToString("dd.MM.yyyy. HH:mm:ss"));

            var query = new GetAccommodationByGuestReservationsQuery(guestEmail);
            var result = await _mediator.Send(query);
            AccommodationCounter.WithLabels("accommodation", "get_accommodation_by_guest_reservations", "200").Inc();
            return Ok(result.ToList());
        }

        [HttpPost]
        public async Task<ActionResult<Accommodation>> CreateAccommodation([FromBody] AccommodationDTO createAccommodationDto)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("Create accommodation");
            Console.SetOut(Console.Out);
            Console.WriteLine("METHOD: POST, DESCRIPTION: Create accommodation, TIME: " + DateTime.Now.ToString("dd.MM.yyyy. HH:mm:ss"));

            if (!ModelState.IsValid)
            {
                AccommodationCounter.WithLabels("accommodation", "create_accommodation", "400").Inc();
                return BadRequest("Invalid request");
            }
            var command = new CreateAccommodationCommand(
                createAccommodationDto
                );

            var result = await _mediator.Send(command);
            AccommodationCounter.WithLabels("accommodation", "create_accommodation", "201").Inc();
            return Created("neki uri", result);
        }

        [HttpGet]
        [Route("benefits")]
        public ActionResult<Benefit> GetBenefits()
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("Get all accommodation benefits");
            Console.SetOut(Console.Out);
            Console.WriteLine("METHOD: GET, DESCRIPTION: Get all accommodation benefits, TIME: " + DateTime.Now.ToString("dd.MM.yyyy. HH:mm:ss"));

            List<string> benefitList = new List<string>();

            foreach (Benefit benefit in Enum.GetValues(typeof(Benefit)))
            {
                benefitList.Add(benefit.ToString());
            }
            AccommodationCounter.WithLabels("accommodation", "get_benefits", "200").Inc();
            return Ok(benefitList);
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<List<AccommodationGetAllDTO>>> SearchAccommodationAsync([FromQuery(Name = "address")] string address, [FromQuery(Name = "numberOfGuests")] int numberOfGuests, [FromQuery(Name = "startDate")] string startDate, [FromQuery(Name = "endDate")] string endDate )
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("Search accommodation");
            Console.SetOut(Console.Out);
            Console.WriteLine("METHOD: GET, DESCRIPTION: Search accommodation, TIME: " + DateTime.Now.ToString("dd.MM.yyyy. HH:mm:ss"));

            if (!ModelState.IsValid)
            {
                AccommodationCounter.WithLabels("accommodation", "search_accommodation", "400").Inc();
                return BadRequest("Invalid request");
            }
            var query = new SearchAccommodationQuery(address, numberOfGuests, startDate, endDate);
            var result = await _mediator.Send(query);
            AccommodationCounter.WithLabels("accommodation", "search_accommodation", "200").Inc();
            return Ok(result.ToList());
        }

        [HttpGet]
        [Route("{adminEmail}/admin-accommodation")]
        public async Task<ActionResult<List<AccommodationGetAllDTO>>> GetAccommodationByAdmin([FromRoute(Name = "adminEmail"), Required] string adminEmail)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("Get all accommodation by host");
            Console.SetOut(Console.Out);
            Console.WriteLine("METHOD: GET, DESCRIPTION: Get all accommodation by host, TIME: " + DateTime.Now.ToString("dd.MM.yyyy. HH:mm:ss"));

            var query = new GetAllAccommodationByAdminQuery(adminEmail);
            var result = await _mediator.Send(query);
            AccommodationCounter.WithLabels("accommodation", "get_accommodation_by_host", "200").Inc();
            return Ok(result.ToList());
        }

        [HttpPost]
        [Route("add-price")]
        public async Task<ActionResult<Accommodation>> AddNewPriceAsync(PriceDTO priceDTO)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("Create new price for accommodation");
            Console.SetOut(Console.Out);
            Console.WriteLine("METHOD: POST, DESCRIPTION: Create new price for accommodation, TIME: " + DateTime.Now.ToString("dd.MM.yyyy. HH:mm:ss"));

            if (!ModelState.IsValid)
            {
                AccommodationCounter.WithLabels("accommodation", "add_new_price", "400").Inc();
                return BadRequest("Invalid request");
            }
            var command= new AddPriceCommand(priceDTO);
            var result = await _mediator.Send(command);
            AccommodationCounter.WithLabels("accommodation", "add_new_price", "200").Inc();
            return Ok(result);
        }

        [HttpGet]
        [Route("filter")]
        public async Task<ActionResult<List<AccommodationGetAllDTO>>> FilterAccommodationAsync([FromQuery(Name = "minPrice")] int minPrice, [FromQuery(Name = "maxPrice")] int maxPrice, [FromQuery(Name = "benefits")] List<Benefit> benefits, [FromQuery(Name = "date")] string date, [FromQuery(Name = "isHighlighted")] bool isHighlighted)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("Filter accommodation");
            Console.SetOut(Console.Out);
            Console.WriteLine("METHOD: GET, DESCRIPTION: Filter accommodation, TIME: " + DateTime.Now.ToString("dd.MM.yyyy. HH:mm:ss"));

            if (!ModelState.IsValid)
            {
                AccommodationCounter.WithLabels("accommodation", "filter_accommodation", "400").Inc();
                return BadRequest("Invalid request");
            }
            var query = new FilterAccommodationQuery(maxPrice, minPrice, benefits, isHighlighted, date);
            var result = await _mediator.Send(query);
            if (!isHighlighted)
            {
                AccommodationCounter.WithLabels("accommodation", "filter_accommodation", "200").Inc();
                return Ok(result.ToList());
            }
            List<AccommodationGetAllDTO> returnList = new List<AccommodationGetAllDTO>();
            foreach(var acc in result.ToList())
            {
                var hostQuery = new CheckHighlightedHostQuery(acc.HostEmail);
                var hostResult = await _mediator.Send(hostQuery);
                if (hostResult)
                    returnList.Add(acc);
            }
            AccommodationCounter.WithLabels("accommodation", "filter_accommodation", "200").Inc();
            return Ok(returnList);
        }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FlightsBooking.Grpc.Protos;
using FlightsBooking.Models;
using FlightsBooking.Services;
using Grpc.Core;
using Rs.Ac.Uns.Ftn.Grpc;

namespace Notification.Application.Notification.Support.Grpc
{
    public class SuggestFlightsServerGrpcServiceImpl : SuggestFlightsGrpcService.SuggestFlightsGrpcServiceBase
    {
        IFlightService _flightService;
        public SuggestFlightsServerGrpcServiceImpl(IFlightService flightService)
        {
            _flightService = flightService;
        }
        public override async Task<GetAllSuggestedFlightsMessageProtoResponse> getAllSuggestedFlights(GetAllSuggestedFlightsMessageProto request, ServerCallContext context)
        {
            var allFlights = await _flightService.GetAsync();
            var flightsForGoing = new List<SuggestedFlightDto>();
            var flightsForReturning = new List<SuggestedFlightDto>();

            foreach (Flight flight in allFlights)
            {
                if (suggestFlightForGoing(flight, DateTime.Parse(request.FirstDayDate)))
                {
                    var suggestedFlightDto = convertFlightToSuggestedFlightDto(flight);
                    flightsForGoing.Add(suggestedFlightDto);
                }
                else if (suggestFlightForReturning(flight, DateTime.Parse(request.LastDayDate)))
                {
                    var suggestedFlightDto = convertFlightToSuggestedFlightDto(flight);
                    flightsForReturning.Add(suggestedFlightDto);
                }
            }
            GetAllSuggestedFlightsMessageProtoResponse response = new GetAllSuggestedFlightsMessageProtoResponse();
            response.FlightsForGoing= JsonSerializer.Serialize(flightsForGoing);
            response.FlightsForReturning= JsonSerializer.Serialize(flightsForReturning);
            return response;
        }

        private bool suggestFlightForGoing(Flight flight, DateTime goingDate)
        {
            return flight.Arrival.Time.Date == goingDate.Date;
        }
        private bool suggestFlightForReturning(Flight flight, DateTime returningDate)
        {
            return flight.Departure.Time.Date == returningDate.Date;
        }
        private SuggestedFlightDto convertFlightToSuggestedFlightDto(Flight flight)
        {
            SuggestedFlightDto suggestedFlightDto = new SuggestedFlightDto();
            suggestedFlightDto.Id = flight.Id;
            suggestedFlightDto.DepartureDate = flight.Departure.Time;
            suggestedFlightDto.ArrivalDate = flight.Arrival.Time;
            suggestedFlightDto.TicketPrice = flight.TicketPrice;
            return suggestedFlightDto;
        }
    }

    public class SuggestedFlightDto
    {
        public Guid Id { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public double TicketPrice { get; set; }
    }
}

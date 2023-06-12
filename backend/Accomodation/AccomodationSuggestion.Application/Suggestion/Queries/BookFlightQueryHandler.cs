using AccommodationSuggestionApplication.Suggestion.Support.Grpc.Protos;
using AccomodationApplication.Abstractions.Messaging;
using AccomodationSuggestion.Application.Dtos;
using Grpc.Core;
using Rs.Ac.Uns.Ftn.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AccomodationSuggestion.Application.Suggestion.Queries
{
    public sealed class BookFlightQueryHandler : IQueryHandler<BookFlightQuery, bool>
    {
        private Channel channel;
        private BookFlightGrpcService.BookFlightGrpcServiceClient client;
        public BookFlightQueryHandler() { }
        public async Task<bool> Handle(BookFlightQuery request, CancellationToken cancellationToken)
        {
            channel = new Channel("localhost:6000", ChannelCredentials.Insecure);
            client = new BookFlightGrpcService.BookFlightGrpcServiceClient(channel);
            BookFlightMessageProtoResponse response = await client.bookFlightAsync(new BookFlightMessageProto()
            {
                FlightId = request.bookFlightDto.FlightId,
                NumberOfTickets = request.bookFlightDto.NumberOfTickets.ToString(),
                UserId = request.bookFlightDto.UserId
            });
            return bool.Parse(response.IsBooked);
        }
    }
    //public sealed class GetAllSuggestedFlightsQueryHandler : IQueryHandler<GetAllSuggestedFlightsQuery, GetAllSuggestedFlightsResponse>
    //{
    //    private Channel channel;
    //    private SuggestFlightsGrpcService.SuggestFlightsGrpcServiceClient client;
    //    public GetAllSuggestedFlightsQueryHandler() { }

    //    public async Task<GetAllSuggestedFlightsResponse> Handle(GetAllSuggestedFlightsQuery request, CancellationToken cancellationToken)
    //    {
    //        channel = new Channel("localhost:6000", ChannelCredentials.Insecure);
    //        client = new SuggestFlightsGrpcService.SuggestFlightsGrpcServiceClient(channel);
    //        GetAllSuggestedFlightsMessageProtoResponse response = await client.getAllSuggestedFlightsAsync(new GetAllSuggestedFlightsMessageProto()
    //        {
    //            FirstDayDate = request.queryDto.FirstDayDate.ToString(),
    //            LastDayDate = request.queryDto.LastDayDate.ToString(),
    //            PlaceOfArrival = request.queryDto.PlaceOfArrival,
    //            PlaceOfDeparture = request.queryDto.PlaceOfDeparture
    //        });
    //        GetAllSuggestedFlightsResponse returnValue = new GetAllSuggestedFlightsResponse(JsonSerializer.Deserialize<List<SuggestedFlightDto>>(response.FlightsForGoing), JsonSerializer.Deserialize<List<SuggestedFlightDto>>(response.FlightsForReturning)) { };
    //        return returnValue;
    //    }
    //}
}

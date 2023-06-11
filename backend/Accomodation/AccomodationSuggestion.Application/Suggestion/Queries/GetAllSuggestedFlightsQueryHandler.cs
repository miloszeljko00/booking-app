using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AccomodationSuggestion.Application.Dtos;
using Grpc.Core;
using AccommodationSuggestionApplication.Suggestion.Support.Grpc.Protos;

namespace AccomodationSuggestion.Application.Suggestion.Queries
{
    public sealed class GetAllSuggestedFlightsQueryHandler
    {
        private Channel channel;
        public GetAllSuggestedFlightsQueryHandler(){}

        public async Task<GetAllSuggestedFlightsResponse> Handle(GetAllSuggestedFlightsQuery request, CancellationToken cancellationToken)
        {
            channel = new Channel("127.0.0.1:8700", ChannelCredentials.Insecure);
            client = new SuggestedFlightService.SuggestedFlightGrpcServiceClient(channel);
            GetAllSuggestedFlightsMessageProto response = await client.suggestedFlightsAsync(new GetAllSuggestedFlightsMessageProto() { 
                   request.queryDto.ArrivalDate,
                   request.queryDto.DepartureDate,
                   request.queryDto.PlaceOfDeparture,
                   request.queryDto.PlaceOfArrival
            });

            channel = new Channel("127.0.0.1:8792", ChannelCredentials.Insecure);
            client1 = new HighlightedHostGrpcService.HighlightedHostGrpcServiceClient(channel);
            MessageResponseProto7 response1;
            if (cancellationRate(request.hostEmail, accommodations) < 0.05 && numberOfSuccessfulReservationsInPast(request.hostEmail, accommodations) >= 5
                && numberOfDaysForSuccessfulReservationsInPast(request.hostEmail, accommodations) > 50 && averageGrade > 4.7)
            {
                response1 = await client1.checkAsync(new MessageProto7() { Email = request.hostEmail, Status = "DOBILI" });
                Console.WriteLine("Email status: " + response1.Status);
                return true;
            }
            response1 = await client1.checkAsync(new MessageProto7() { Email = request.hostEmail, Status = "IZGUBILI" });
            Console.WriteLine("Email status: " + response1.Status);
            return false;
        }
    }
}

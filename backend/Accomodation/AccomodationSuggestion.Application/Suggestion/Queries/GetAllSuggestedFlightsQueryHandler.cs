using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AccomodationSuggestion.Application.Dtos;
using Grpc.Core;
using AccommodationSuggestionApplication.Suggestion.Support.Grpc.Protos;
using Rs.Ac.Uns.Ftn.Grpc;
using System.Text.Json;
using AccomodationApplication.Abstractions.Messaging;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Grpc.Net.Client.Web;

namespace AccomodationSuggestion.Application.Suggestion.Queries
{
    public sealed class GetAllSuggestedFlightsQueryHandler : IQueryHandler<GetAllSuggestedFlightsQuery, GetAllSuggestedFlightsResponse>
    {
        private Channel channel;
        private SuggestFlightsGrpcService.SuggestFlightsGrpcServiceClient client;
        private IConfiguration _configuration;
        private IHostEnvironment _env;
        public GetAllSuggestedFlightsQueryHandler(IConfiguration configuration, IHostEnvironment env) {
            _configuration=configuration;
            _env=env;
        }

        public async Task<GetAllSuggestedFlightsResponse> Handle(GetAllSuggestedFlightsQuery request, CancellationToken cancellationToken)
        {
            if (_env.EnvironmentName != "Cloud")
            {
                
                var channel = new Channel(_configuration.GetValue<string>("GrpcDruzina:Letici:Address") + ":" + _configuration.GetValue<int>("GrpcDruzina:Letici:Port"), ChannelCredentials.Insecure);
                client = new SuggestFlightsGrpcService.SuggestFlightsGrpcServiceClient(channel);
                GetAllSuggestedFlightsMessageProtoResponse response = await client.getAllSuggestedFlightsAsync(new GetAllSuggestedFlightsMessageProto()
                {
                    FirstDayDate = request.queryDto.FirstDayDate.ToString(),
                    LastDayDate = request.queryDto.LastDayDate.ToString(),
                    PlaceOfArrival = request.queryDto.PlaceOfArrival,
                    PlaceOfDeparture = request.queryDto.PlaceOfDeparture
                });
                GetAllSuggestedFlightsResponse returnValue = new GetAllSuggestedFlightsResponse(JsonSerializer.Deserialize<List<SuggestedFlightDto>>(response.FlightsForGoing), JsonSerializer.Deserialize<List<SuggestedFlightDto>>(response.FlightsForReturning)) { };
                return returnValue;
            }
            else
            {
                Console.WriteLine("*****POMAGAJ BOZE*****");
                Console.WriteLine((_configuration.GetValue<string>("GrpcDruzina:Letici:Address")));
                using var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("GrpcDruzina:Letici:Address"), new GrpcChannelOptions
                {
                    HttpHandler = new GrpcWebHandler(new HttpClientHandler())
                });
                client = new SuggestFlightsGrpcService.SuggestFlightsGrpcServiceClient(channel);
                GetAllSuggestedFlightsMessageProtoResponse response = await client.getAllSuggestedFlightsAsync(new GetAllSuggestedFlightsMessageProto()
                {
                    FirstDayDate = request.queryDto.FirstDayDate.ToString(),
                    LastDayDate = request.queryDto.LastDayDate.ToString(),
                    PlaceOfArrival = request.queryDto.PlaceOfArrival,
                    PlaceOfDeparture = request.queryDto.PlaceOfDeparture
                });
                GetAllSuggestedFlightsResponse returnValue = new GetAllSuggestedFlightsResponse(JsonSerializer.Deserialize<List<SuggestedFlightDto>>(response.FlightsForGoing), JsonSerializer.Deserialize<List<SuggestedFlightDto>>(response.FlightsForReturning)) { };
                return returnValue;
            }
            
        }
    }
}

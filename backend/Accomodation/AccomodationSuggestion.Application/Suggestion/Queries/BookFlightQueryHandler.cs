using AccommodationSuggestionApplication.Suggestion.Support.Grpc.Protos;
using AccomodationApplication.Abstractions.Messaging;
using AccomodationSuggestion.Application.Dtos;
using Grpc.Core;
using Grpc.Net.Client.Web;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Rs.Ac.Uns.Ftn.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace AccomodationSuggestion.Application.Suggestion.Queries
{
    public sealed class BookFlightQueryHandler : IQueryHandler<BookFlightQuery, bool>
    {
        private BookFlightGrpcService.BookFlightGrpcServiceClient client;
        private IConfiguration _configuration;
        private IHostEnvironment _env;
        public BookFlightQueryHandler(IHostEnvironment env, IConfiguration configuration) {
            _configuration = configuration;
            _env = env;
        }
        public async Task<bool> Handle(BookFlightQuery request, CancellationToken cancellationToken)
        {
            if (_env.EnvironmentName != "Cloud")
            {
                var channel = new Channel(_configuration.GetValue<string>("GrpcDruzina:Letici:Address") + ":" + _configuration.GetValue<int>("GrpcDruzina:Letici:Port"), ChannelCredentials.Insecure);
                client = new BookFlightGrpcService.BookFlightGrpcServiceClient(channel);
                BookFlightMessageProtoResponse response = await client.bookFlightAsync(new BookFlightMessageProto()
                {
                    FlightId = request.bookFlightDto.FlightId,
                    NumberOfTickets = request.bookFlightDto.NumberOfTickets.ToString(),
                    UserId = request.bookFlightDto.UserId,
                    ApiKey = request.bookFlightDto.ApiKey
                });
                return bool.Parse(response.IsBooked);
            }
            else
            {
                using var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("GrpcDruzina:Letici:Address"), new GrpcChannelOptions
                {
                    HttpHandler = new GrpcWebHandler(new HttpClientHandler())
                });
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
    }
}

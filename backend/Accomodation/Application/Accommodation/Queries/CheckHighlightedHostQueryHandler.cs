using AccommodationApplication.Accommodation.Support.Grpc.Protos;
using Accomodation.Application.Dtos;
using Accomodation.Domain.Primitives.Enums;
using AccomodationApplication.Abstractions.Messaging;
using AccomodationSuggestionDomain.Entities;
using AccomodationSuggestionDomain.Interfaces;
using Grpc.Core;
using Grpc.Net.Client.Web;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace AccomodationApplication.Accommodation.Queries
{
    public sealed class CheckHighlightedHostQueryHandler : IQueryHandler<CheckHighlightedHostQuery, bool>
    {
        private readonly IAccommodationRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _env;

        private HostGradeGrpcService.HostGradeGrpcServiceClient client;

        private HighlightedHostGrpcService.HighlightedHostGrpcServiceClient client1;
        public CheckHighlightedHostQueryHandler(IAccommodationRepository repository, IConfiguration configuration, IHostEnvironment env)
        {
            _repository = repository;
            _configuration = configuration;
            _env = env;
        }

        public async Task<bool> Handle(CheckHighlightedHostQuery request, CancellationToken cancellationToken)
        {
            var accs = await _repository.GetAllAsync();
            List<AccomodationSuggestionDomain.Entities.Accommodation> accommodations = accs.ToList();
            Console.WriteLine("Cancellation rate: " + cancellationRate(request.hostEmail, accommodations));
            Console.WriteLine("Successful reservations: " + numberOfSuccessfulReservationsInPast(request.hostEmail, accommodations));
            Console.WriteLine("Days of reservations: " + numberOfDaysForSuccessfulReservationsInPast(request.hostEmail, accommodations));

            if (_env.EnvironmentName != "Cloud")
            {
                var channel = new Channel(_configuration.GetValue<string>("GrpcDruzina:AccommodationGrading:Address") + ":" + _configuration.GetValue<int>("GrpcDruzina:AccommodationGrading:Port"), ChannelCredentials.Insecure);
                client = new HostGradeGrpcService.HostGradeGrpcServiceClient(channel);
                MessageResponseProto6 response = await client.hostGradeAsync(new MessageProto6() { Email = request.hostEmail });
                double averageGrade = response.Grade;

                var channel1 = new Channel(_configuration.GetValue<string>("GrpcDruzina:Notification:Address") + ":" + _configuration.GetValue<int>("GrpcDruzina:Notification:Port"), ChannelCredentials.Insecure);
                client1 = new HighlightedHostGrpcService.HighlightedHostGrpcServiceClient(channel1);
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
            else
            {
                using var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("GrpcDruzina:AccommodationGrading:Address"), new GrpcChannelOptions
                {
                    HttpHandler = new GrpcWebHandler(new HttpClientHandler())
                });
                client = new HostGradeGrpcService.HostGradeGrpcServiceClient(channel);
                MessageResponseProto6 response = await client.hostGradeAsync(new MessageProto6() { Email = request.hostEmail });
                double averageGrade = response.Grade;

                using var channel1 = GrpcChannel.ForAddress(_configuration.GetValue<string>("GrpcDruzina:Notification:Address"), new GrpcChannelOptions
                {
                    HttpHandler = new GrpcWebHandler(new HttpClientHandler())
                });
                client1 = new HighlightedHostGrpcService.HighlightedHostGrpcServiceClient(channel1);
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

        private int numberOfSuccessfulReservationsInPast(string hostEmail, List<AccomodationSuggestionDomain.Entities.Accommodation> accommodations)
        {
            int numberOfSuccessfulReservationsInPast = 0;
            foreach (var acc in accommodations)
            {
                if (acc.HostEmail.EmailAddress.Equals(hostEmail))
                {
                    numberOfSuccessfulReservationsInPast += acc.GetNumberOfSuccessfulReservationsInPast();
                }
            }
            return numberOfSuccessfulReservationsInPast;
        }

        private int numberOfDaysForSuccessfulReservationsInPast(string hostEmail, List<AccomodationSuggestionDomain.Entities.Accommodation> accommodations)
        {
            int numberOfDaysForSuccessfulReservationsInPast = 0;
            foreach (var acc in accommodations)
            {
                if (acc.HostEmail.EmailAddress.Equals(hostEmail))
                {
                    numberOfDaysForSuccessfulReservationsInPast += acc.GetNumberOfDaysForSuccessfulReservationsInPast();
                }
            }
            return numberOfDaysForSuccessfulReservationsInPast;
        }

        private double cancellationRate(string hostEmail, List<AccomodationSuggestionDomain.Entities.Accommodation> accommodations)
        {
            int canceledReservations = 0;
            int numberOfReservations = 0;
            foreach (var acc in accommodations)
            {
                if (acc.HostEmail.EmailAddress.Equals(hostEmail))
                {
                    canceledReservations += acc.GetCancellationNumber();
                    numberOfReservations += acc.Reservations.Count;
                }
            }
            if (numberOfReservations == 0)
                return 1;
            return (double)canceledReservations / numberOfReservations;
        }
    }
}

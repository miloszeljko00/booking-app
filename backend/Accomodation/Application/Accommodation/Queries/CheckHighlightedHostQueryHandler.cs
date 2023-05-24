using AccommodationApplication.Accommodation.Support.Grpc.Protos;
using Accomodation.Application.Dtos;
using Accomodation.Domain.Primitives.Enums;
using AccomodationApplication.Abstractions.Messaging;
using AccomodationSuggestionDomain.Entities;
using AccomodationSuggestionDomain.Interfaces;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationApplication.Accommodation.Queries
{
    public sealed class CheckHighlightedHostQueryHandler : IQueryHandler<CheckHighlightedHostQuery, bool>
    {
        private readonly IAccommodationRepository _repository;

        private Channel channel;

        private HostGradeGrpcService.HostGradeGrpcServiceClient client;
        public CheckHighlightedHostQueryHandler(IAccommodationRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(CheckHighlightedHostQuery request, CancellationToken cancellationToken)
        {
            var accs = await _repository.GetAllAsync();
            List<AccomodationSuggestionDomain.Entities.Accommodation> accommodations = accs.ToList();
            Console.WriteLine("Cancellation rate: " + cancellationRate(request.hostEmail, accommodations));
            Console.WriteLine("Successful reservations: " + numberOfSuccessfulReservationsInPast(request.hostEmail, accommodations));
            Console.WriteLine("Days of reservations: " + numberOfDaysForSuccessfulReservationsInPast(request.hostEmail, accommodations));

            channel = new Channel("127.0.0.1:8700", ChannelCredentials.Insecure);
            client = new HostGradeGrpcService.HostGradeGrpcServiceClient(channel);
            MessageResponseProto6 response = await client.hostGradeAsync(new MessageProto6() { Email = request.hostEmail });
            double averageGrade = response.Grade;
            Console.WriteLine("Host average rating is: " + averageGrade);

            if (cancellationRate(request.hostEmail, accommodations) < 0.05 && numberOfSuccessfulReservationsInPast(request.hostEmail, accommodations) >= 5
                && numberOfDaysForSuccessfulReservationsInPast(request.hostEmail, accommodations) > 50 && averageGrade > 4.7)
                return true;
            return false;
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

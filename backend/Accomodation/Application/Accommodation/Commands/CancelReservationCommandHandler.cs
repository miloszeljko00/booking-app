using AccommodationApplication.Accommodation.Support.Grpc.Protos;
using Accomodation.Domain.Primitives.Enums;
using AccomodationApplication.Abstractions.Messaging;
using AccomodationSuggestionDomain.Entities;
using AccomodationSuggestionDomain.Interfaces;
using AccomodationSuggestionDomain.Primitives.Enums;
using AccomodationSuggestionDomain.ValueObjects;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationApplication.Accommodation.Commands
{
    public sealed class CancelReservationCommandHandler : ICommandHandler<CancelReservationCommand, AccomodationSuggestionDomain.Entities.Accommodation>
    {
        private readonly IAccommodationRepository _repository;

        private Channel channel;

        private HostCancelReservationNotificationGrpcService.HostCancelReservationNotificationGrpcServiceClient client;
        public AccommodationBuilder AccommodationBuilder { get; set; } = new AccommodationBuilder();
        public CancelReservationCommandHandler(IAccommodationRepository repository)
        {
            _repository = repository;
        }

        public IAccommodationRepository Get_repository()
        {
            return _repository;
        }

        public async Task<AccomodationSuggestionDomain.Entities.Accommodation> Handle(CancelReservationCommand request, CancellationToken cancellationToken)
        {
            var acc = _repository.GetAsync(request.reservationCancellationDTO.AccommodationId);
            AccomodationSuggestionDomain.Entities.Accommodation accommodation = acc.Result;
            if(accommodation is null) 
            {
                throw new Exception("Accommodation does not exist");
            }
            List<ReservationRequest> reservationRequests = accommodation.ReservationRequests;
            List<Reservation> reservations = accommodation.Reservations;
            ReservationRequest req = null;
            Reservation res = null;
            foreach (Reservation reservation in reservations)
            {
                if (reservation.Id.Equals(request.reservationCancellationDTO.ReservationId))
                {
                    res = reservation;
                    reservations.Remove(reservation);
                    break;
                }
            }
            foreach (ReservationRequest reservationRequest in reservationRequests)
            {
                if (reservationRequest.ReservationDate.Start.Equals(res.ReservationDate.Start) && reservationRequest.ReservationDate.End.Equals(res.ReservationDate.End))
                {
                    req = reservationRequest;
                    reservationRequests.Remove(reservationRequest);
                    break;
                }
            }
            if (req is not null && res is not null)
            {
                accommodation.CreateReservationRequest(req.GuestEmail.EmailAddress, req.ReservationDate.Start, req.ReservationDate.End, req.GuestNumber, ReservationRequestStatus.CANCELED);
                accommodation.CreateReservation(res.GuestEmail.EmailAddress, res.ReservationDate.Start, res.ReservationDate.End, res.GuestNumber, accommodation.PriceCalculation == PriceCalculation.PER_PERSON ? true : false, (int)accommodation.GetPriceForSpecificDate(res.ReservationDate.Start).Value, true);
            }
            else
                throw new Exception("Reservation or its request does not exist");

            channel = new Channel("127.0.0.1:8789", ChannelCredentials.Insecure);
            client = new HostCancelReservationNotificationGrpcService.HostCancelReservationNotificationGrpcServiceClient(channel);
            MessageResponseProto3 response = await client.communicateAsync(new MessageProto3() { Email = accommodation.HostEmail.EmailAddress, Accommodation = accommodation.Name, StartDate = res.ReservationDate.Start.ToString("dd.MM.yyy."), EndDate = res.ReservationDate.End.ToString("dd.MM.yyy.") });
            Console.WriteLine(response);

            _repository.UpdateAsync(accommodation.Id, accommodation);
            return accommodation;
        }
    }
}

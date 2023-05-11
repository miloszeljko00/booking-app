using Accomodation.Domain.Primitives.Enums;
using AccomodationApplication.Abstractions.Messaging;
using AccomodationDomain.Entities;
using AccomodationDomain.Interfaces;
using AccomodationDomain.Primitives.Enums;
using AccomodationDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationApplication.Accommodation.Commands
{
    public sealed class CancelReservationCommandHandler : ICommandHandler<CancelReservationCommand, AccomodationDomain.Entities.Accommodation>
    {
        private readonly IAccommodationRepository _repository;
        public AccommodationBuilder AccommodationBuilder { get; set; } = new AccommodationBuilder();
        public CancelReservationCommandHandler(IAccommodationRepository repository)
        {
            _repository = repository;
        }

        public IAccommodationRepository Get_repository()
        {
            return _repository;
        }

        public Task<AccomodationDomain.Entities.Accommodation> Handle(CancelReservationCommand request, CancellationToken cancellationToken)
        {
            var acc = _repository.GetAsync(request.reservationCancellationDTO.AccommodationId);
            AccomodationDomain.Entities.Accommodation accommodation = acc.Result;
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
            _repository.UpdateAsync(accommodation.Id, accommodation);
            return Task.FromResult(accommodation);
        }
    }
}

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
    public sealed class ManageReservationRequestCommandHandler : ICommandHandler<ManageReservationRequestCommand, AccomodationDomain.Entities.Accommodation>
    {
        private readonly IAccommodationRepository _repository;
        public AccommodationBuilder AccommodationBuilder { get; set; } = new AccommodationBuilder();
        public ManageReservationRequestCommandHandler(IAccommodationRepository repository)
        {
            _repository = repository;
        }

        public IAccommodationRepository Get_repository()
        {
            return _repository;
        }

        public Task<AccomodationDomain.Entities.Accommodation> Handle(ManageReservationRequestCommand request, CancellationToken cancellationToken)
        {
            var acc = _repository.GetAsync(request.reservationRequestManagementDTO.AccommodationId);
            AccomodationDomain.Entities.Accommodation accommodation = acc.Result;
            if(accommodation is null) 
            {
                throw new Exception("Accommodation does not exist");
            }
            List<ReservationRequest> reservationRequests = accommodation.ReservationRequests;
            ReservationRequest req = null;
            foreach (ReservationRequest reservationRequest in reservationRequests)
            {
                if (reservationRequest.Id.Equals(request.reservationRequestManagementDTO.ReservationId))
                {
                    req = reservationRequest;
                    reservationRequests.Remove(reservationRequest);
                    break;
                }
            }

            if (req is not null)
            {
                if (request.reservationRequestManagementDTO.Operation.Equals("ACCEPT"))
                {
                    List<ReservationRequest> reqs = accommodation.GetReservationRequestsOverlappingDateRange(req.ReservationDate);
                    foreach (ReservationRequest reservationRequest in reqs)
                    {
                        reservationRequests.Remove(reservationRequest);
                        accommodation.CreateReservationRequest(reservationRequest.GuestEmail.EmailAddress, reservationRequest.ReservationDate.Start, reservationRequest.ReservationDate.End, reservationRequest.GuestNumber, ReservationRequestStatus.REJECTED);
                    }
                    accommodation.CreateReservationRequest(req.GuestEmail.EmailAddress, req.ReservationDate.Start, req.ReservationDate.End, req.GuestNumber, ReservationRequestStatus.ACCEPTED);
                    accommodation.CreateReservation(req.GuestEmail.EmailAddress, req.ReservationDate.Start, req.ReservationDate.End, req.GuestNumber, accommodation.PriceCalculation == PriceCalculation.PER_PERSON ? true : false, (int)accommodation.GetPriceForSpecificDate(req.ReservationDate.Start).Value, false);
                }
                else if (request.reservationRequestManagementDTO.Operation.Equals("REJECT"))
                {
                    accommodation.CreateReservationRequest(req.GuestEmail.EmailAddress, req.ReservationDate.Start, req.ReservationDate.End, req.GuestNumber, ReservationRequestStatus.REJECTED);
                }
                else
                {
                    throw new Exception("Invalid operation selected");
                }
            }
            else
                throw new Exception("Reservation request does not exist");

            _repository.UpdateAsync(accommodation.Id, accommodation);
            return Task.FromResult(accommodation);
        }
    }
}

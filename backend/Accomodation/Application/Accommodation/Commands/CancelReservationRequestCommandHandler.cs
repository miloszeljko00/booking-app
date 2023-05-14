using Accomodation.Domain.Primitives.Enums;
using AccomodationApplication.Abstractions.Messaging;
using AccomodationSuggestionDomain.Entities;
using AccomodationSuggestionDomain.Interfaces;
using AccomodationSuggestionDomain.Primitives.Enums;
using AccomodationSuggestionDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationApplication.Accommodation.Commands
{
    public sealed class CancelReservationRequestCommandHandler : ICommandHandler<CancelReservationRequestCommand, AccomodationSuggestionDomain.Entities.Accommodation>
    {
        private readonly IAccommodationRepository _repository;
        public AccommodationBuilder AccommodationBuilder { get; set; } = new AccommodationBuilder();
        public CancelReservationRequestCommandHandler(IAccommodationRepository repository)
        {
            _repository = repository;
        }

        public IAccommodationRepository Get_repository()
        {
            return _repository;
        }

        public Task<AccomodationSuggestionDomain.Entities.Accommodation> Handle(CancelReservationRequestCommand request, CancellationToken cancellationToken)
        {
            var acc = _repository.GetAsync(request.reservationRequestCancellationDTO.AccommodationId);
            AccomodationSuggestionDomain.Entities.Accommodation accommodation = acc.Result;
            if(accommodation is null) 
            {
                throw new Exception("Accommodation does not exist");
            }
            List<ReservationRequest> reservationRequests = accommodation.ReservationRequests;
            ReservationRequest req = null;
            foreach (ReservationRequest reservationRequest in reservationRequests)
            {
                if (reservationRequest.Id.Equals(request.reservationRequestCancellationDTO.ReservationId))
                {
                    req = reservationRequest;
                    reservationRequests.Remove(reservationRequest);
                    break;
                }
            }
            if(req is not null)
                accommodation.CreateReservationRequest(req.GuestEmail.EmailAddress, req.ReservationDate.Start, req.ReservationDate.End, req.GuestNumber, ReservationRequestStatus.CANCELED);
            else
                throw new Exception("Reservation request does not exist");
            _repository.UpdateAsync(accommodation.Id, accommodation);
            return Task.FromResult(accommodation);
        }
    }
}

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
    public sealed class CreateReservationRequestCommandHandler : ICommandHandler<CreateReservationRequestCommand, AccomodationSuggestionDomain.Entities.Accommodation>
    {
        private readonly IAccommodationRepository _repository;
        public AccommodationBuilder AccommodationBuilder { get; set; } = new AccommodationBuilder();
        public CreateReservationRequestCommandHandler(IAccommodationRepository repository)
        {
            _repository = repository;
        }

        public IAccommodationRepository Get_repository()
        {
            return _repository;
        }

        public Task<AccomodationSuggestionDomain.Entities.Accommodation> Handle(CreateReservationRequestCommand request, CancellationToken cancellationToken)
        {
            var acc = _repository.GetAsync(request.reservationRequestDTO.AccommodationId);
            AccomodationSuggestionDomain.Entities.Accommodation accommodation = acc.Result;
            if(accommodation is null) 
            {
                throw new Exception("Accommodation does not exist");
            }
            if (!accommodation.IsDateRangeOfReservationValid(DateRange.Create(request.reservationRequestDTO.Start, request.reservationRequestDTO.End)))
            {
                throw new Exception("Accommodation is not available in this date range");
            }
            if (accommodation.IsReservationDateRangeTaken(DateRange.Create(request.reservationRequestDTO.Start, request.reservationRequestDTO.End)))
            {
                throw new Exception("This date range is already reserved");
            }
            if (accommodation.ReserveAutomatically)
            {
                bool isPerPerson = accommodation.PriceCalculation == PriceCalculation.PER_PERSON ? true : false;
                accommodation.CreateReservationRequest(request.reservationRequestDTO.GuestEmail, request.reservationRequestDTO.Start, request.reservationRequestDTO.End, request.reservationRequestDTO.numberOfGuests, ReservationRequestStatus.ACCEPTED);
                accommodation.CreateReservation(request.reservationRequestDTO.GuestEmail, request.reservationRequestDTO.Start, request.reservationRequestDTO.End, request.reservationRequestDTO.numberOfGuests, isPerPerson, (int)accommodation.GetPriceForSpecificDate(request.reservationRequestDTO.Start).Value, false);
            }
            else
            {
                accommodation.CreateReservationRequest(request.reservationRequestDTO.GuestEmail, request.reservationRequestDTO.Start, request.reservationRequestDTO.End, request.reservationRequestDTO.numberOfGuests, ReservationRequestStatus.PENDING);
            }
            _repository.UpdateAsync(accommodation.Id, accommodation);
            return Task.FromResult(accommodation);
        }
    }
}

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
    public sealed class CreateReservationRequestCommandHandler : ICommandHandler<CreateReservationRequestCommand, AccomodationDomain.Entities.Accommodation>
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

        public Task<AccomodationDomain.Entities.Accommodation> Handle(CreateReservationRequestCommand request, CancellationToken cancellationToken)
        {
            var acc = _repository.GetAsync(request.ReservationRequestDTO.AccommodationId);
            AccomodationDomain.Entities.Accommodation accommodation = acc.Result;
            if(accommodation is null) 
            {
                throw new Exception("Accommodation does not exist");
            }
            if (!accommodation.IsDateRangeOfReservationValid(DateRange.Create(request.ReservationRequestDTO.Start, request.ReservationRequestDTO.End)))
            {
                throw new Exception("Accommodation is not available in this date range");
            }
            if (accommodation.IsReservationDateRangeTaken(DateRange.Create(request.ReservationRequestDTO.Start, request.ReservationRequestDTO.End)))
            {
                throw new Exception("This date range is already reserved");
            }
            if (accommodation.ReserveAutomatically)
            {
                bool isPerPerson = accommodation.PriceCalculation == PriceCalculation.PER_PERSON ? true : false;
                accommodation.CreateReservationRequest(request.ReservationRequestDTO.GuestEmail, request.ReservationRequestDTO.Start, request.ReservationRequestDTO.End, request.ReservationRequestDTO.numberOfGuests, ReservationRequestStatus.ACCEPTED);
                accommodation.CreateReservation(request.ReservationRequestDTO.GuestEmail, request.ReservationRequestDTO.Start, request.ReservationRequestDTO.End, request.ReservationRequestDTO.numberOfGuests, isPerPerson, (int)accommodation.GetPriceForSpecificDate(request.ReservationRequestDTO.Start).Value, false);
            }
            else
            {
                accommodation.CreateReservationRequest(request.ReservationRequestDTO.GuestEmail, request.ReservationRequestDTO.Start, request.ReservationRequestDTO.End, request.ReservationRequestDTO.numberOfGuests, ReservationRequestStatus.PENDING);
            }
            _repository.UpdateAsync(accommodation.Id, accommodation);
            return Task.FromResult(accommodation);
        }
    }
}

using AccomodationApplication.Abstractions.Messaging;
using AccomodationSuggestionDomain.Interfaces;
using AccomodationSuggestionDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accomodation.Application.Accommodation.Commands
{
    public sealed class AddPriceCommandHandler : ICommandHandler<AddPriceCommand, AccomodationSuggestionDomain.Entities.Accommodation>
    {
        private readonly IAccommodationRepository _repository;
        public AddPriceCommandHandler(IAccommodationRepository repository) 
        {
            _repository = repository;
        }

        public Task<AccomodationSuggestionDomain.Entities.Accommodation> Handle(AddPriceCommand request, CancellationToken cancellationToken)
        {
            var acc = _repository.GetAsync(request.priceDTO.AccommodationId);
            AccomodationSuggestionDomain.Entities.Accommodation accommodation = acc.Result;
            if (accommodation is null)
            {
                throw new Exception("Accommodation does not exist");
            }
            string format = "MM/dd/yyyy";
            DateTime _startDate = DateTime.ParseExact(request.priceDTO.StartDate, format, CultureInfo.InvariantCulture);
            DateTime _endDate = DateTime.ParseExact(request.priceDTO.EndDate, format, CultureInfo.InvariantCulture);
            Price price = Price.Create(request.priceDTO.Value, DateRange.Create(_startDate, _endDate));
            if (accommodation.IsReservationDateRangeTaken(price.DateRange))
            {
                throw new Exception("There is a reservation in this DateRange!");
            }
            accommodation.AddNewPrice(price);
            _repository.UpdateAsync(accommodation.Id, accommodation);
            return Task.FromResult(accommodation);
          }
    }

}

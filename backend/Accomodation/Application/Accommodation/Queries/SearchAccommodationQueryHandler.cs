using Accomodation.Application.Dtos;
using AccomodationApplication.Abstractions.Messaging;
using AccomodationApplication.Accommodation.Queries;
using AccomodationApplication.Dtos;
using AccomodationSuggestionDomain.Interfaces;
using AccomodationSuggestionDomain.Primitives.Enums;
using AccomodationSuggestionDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Accomodation.Application.Accommodation.Queries
{
    public sealed class SearchAccommodationQueryHandler : IQueryHandler<SearchAccommodationQuery, ICollection<AccommodationGetAllDTO>>
    {
        private readonly IAccommodationRepository _repository;
        public SearchAccommodationQueryHandler(IAccommodationRepository repository)
        {
            _repository = repository;
        }

        public async Task<ICollection<AccommodationGetAllDTO>> Handle(SearchAccommodationQuery request, CancellationToken cancellationToken)
        {
            string format = "MM/dd/yyyy";
            DateTime _startDate = DateTime.ParseExact(request.startDate, format, CultureInfo.InvariantCulture);
            DateTime _endDate = DateTime.ParseExact(request.endDate, format, CultureInfo.InvariantCulture);
            var accList = await _repository.GetAllAsync();
            ICollection<AccommodationGetAllDTO> result = new Collection<AccommodationGetAllDTO>();
            foreach(var acc in accList){
                if (acc.GetPriceForSpecificDate(_startDate)!= null && !acc.IsReservationDateRangeTaken(DateRange.Create(_startDate, _endDate)) && acc.IsValidNumberOfGuests(request.numberOfGuests) && acc.GetAddressAsString().ToLower().Contains(request.address.ToLower()))
                {
                    AccommodationGetAllDTO dto = new AccommodationGetAllDTO { Name = acc.Name, Address = acc.GetAddressAsString(), Min = acc.Capacity.Min, Max = acc.Capacity.Max,
                        Price = acc.GetPriceForSpecificDate(_startDate).Value, PriceCalculation = acc.PriceCalculation.ToString(), Benefits = acc.GetBenefitsAsString(), Id = acc.Id.ToString(), 
                    HostEmail = acc.HostEmail.EmailAddress};
                    result.Add(dto);
                }

            }
            return result;
        }
    }
}

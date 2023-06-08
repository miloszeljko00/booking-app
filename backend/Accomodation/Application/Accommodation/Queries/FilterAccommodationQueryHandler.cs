using Accomodation.Application.Dtos;
using AccomodationApplication.Abstractions.Messaging;
using AccomodationSuggestionDomain.Interfaces;
using AccomodationSuggestionDomain.Primitives.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accomodation.Application.Accommodation.Queries
{
    public sealed class FilterAccommodationQueryHandler : IQueryHandler<FilterAccommodationQuery, ICollection<AccommodationGetAllDTO>>
    {
        private readonly IAccommodationRepository _repository;
        public FilterAccommodationQueryHandler(IAccommodationRepository repository)
        {
            _repository = repository;
        }
        public async Task<ICollection<AccommodationGetAllDTO>> Handle(FilterAccommodationQuery request, CancellationToken cancellationToken)
        {
            string format = "MM/dd/yyyy";
            DateTime _date = DateTime.ParseExact(request.date, format, CultureInfo.InvariantCulture);
            var accList = await _repository.GetAllAsync();
            ICollection<AccommodationGetAllDTO> result = new Collection<AccommodationGetAllDTO>();
           
          
            foreach (var acc in accList)
            {
                bool allBenefits = true;
                foreach (var benefit in request.benefits)
                {
                    if (!acc.Benefits.Contains(benefit))
                    {
                        allBenefits = false;
                        break;
                    }
                }
                if (!allBenefits)
                    continue;
                var price = acc.GetPriceForSpecificDate(_date);
                if(price != null)
                {
                    if(price.Value > request.minPrice && price.Value < request.maxPrice)
                    {
                        AccommodationGetAllDTO dto = new AccommodationGetAllDTO
                        {
                            Name = acc.Name,
                            Address = acc.GetAddressAsString(),
                            Min = acc.Capacity.Min,
                            Max = acc.Capacity.Max,
                            Price = acc.GetPriceForSpecificDate(_date).Value,
                            PriceCalculation = acc.PriceCalculation.ToString(),
                            Benefits = acc.GetBenefitsAsString(),
                            Id = acc.Id.ToString(),
                            HostEmail = acc.HostEmail.EmailAddress
                        };
                        result.Add(dto);
                    }
                }
            }
            return result;
        }
    }
}

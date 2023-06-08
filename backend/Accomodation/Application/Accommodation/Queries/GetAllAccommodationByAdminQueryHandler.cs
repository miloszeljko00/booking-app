using Accomodation.Application.Dtos;
using AccomodationApplication.Abstractions.Messaging;
using AccomodationSuggestionDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accomodation.Application.Accommodation.Queries
{
    public sealed class GetAllAccommodationByAdminQueryHandler: IQueryHandler<GetAllAccommodationByAdminQuery, ICollection<AccommodationGetAllDTO>>
    {
        private readonly IAccommodationRepository _repository;
        public GetAllAccommodationByAdminQueryHandler(IAccommodationRepository repository)
        {
            _repository = repository;
        }

        public async Task<ICollection<AccommodationGetAllDTO>> Handle(GetAllAccommodationByAdminQuery request, CancellationToken cancellationToken)
        {
            var accList = await _repository.GetAllAsync();
            List<AccomodationSuggestionDomain.Entities.Accommodation> accommodationList = accList.ToList();
            List<AccommodationGetAllDTO> result = new List<AccommodationGetAllDTO>();
            foreach (AccomodationSuggestionDomain.Entities.Accommodation acc in accommodationList)
            {
                if (acc.HostEmail.EmailAddress.Equals(request.adminEmail))
                {
                    var price1 = acc.GetPriceForSpecificDate(DateTime.Now);
                    double price =  price1 == null ? 0 : price1.Value;
                    AccommodationGetAllDTO dto = new AccommodationGetAllDTO { Name = acc.Name, Address = acc.GetAddressAsString(), Min = acc.Capacity.Min, Max = acc.Capacity.Max,
                        Price = price, PriceCalculation = acc.PriceCalculation.ToString(), Benefits = acc.GetBenefitsAsString(), Id = acc.Id.ToString(), HostEmail = acc.HostEmail.EmailAddress };
                    result.Add(dto);
                }
            }

            return result;
        }
    }
}

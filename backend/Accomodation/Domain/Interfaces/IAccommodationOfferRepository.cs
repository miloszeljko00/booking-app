using AccomodationSuggestionDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationSuggestionDomain.Interfaces
{
    public interface IAccommodationOfferRepository
    {
        Task<List<AccommodationOffer>> GetAllAsync();
        Task<AccommodationOffer> Create(AccommodationOffer accommodationOffer);
    }
}

using AccomodationApplication.Abstractions.Messaging;
using AccomodationApplication.Dtos;
using AccomodationSuggestionDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationApplication.Accommodation.Queries
{
    public sealed record GetAccommodationByGuestReservationsQuery(string guestEmail) : IQuery<ICollection<AccommodationMainDTO>>
    {
    }
}

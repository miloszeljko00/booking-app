using Accomodation.Application.Dtos;
using AccomodationApplication.Abstractions.Messaging;
using AccomodationSuggestionDomain.Primitives.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accomodation.Application.Accommodation.Queries
{
    public sealed record FilterAccommodationQuery(int maxPrice, int minPrice, List<Benefit> benefits, bool isHighlighted, string date) : IQuery<ICollection<AccommodationGetAllDTO>>
    {
    }
}

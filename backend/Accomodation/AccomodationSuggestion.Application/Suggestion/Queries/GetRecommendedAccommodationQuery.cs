using AccomodationApplication.Abstractions.Messaging;
using AccomodationSuggestion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationSuggestion.Application.Suggestion.Queries
{
    public sealed record GetRecommendedAccommodationQuery(string guestEmail): IQuery<List<AccommodationNode>>
    {
    }
    
}

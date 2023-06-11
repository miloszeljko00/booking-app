using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationSuggestion.Application.Dtos
{
    public class GetAllSuggestedFlightsResponse
    {
        List<SuggestedFlightDto>? flightsForGoing { get; set; }
        List<SuggestedFlightDto>? flightsForReturning { get; set; }
    }
}

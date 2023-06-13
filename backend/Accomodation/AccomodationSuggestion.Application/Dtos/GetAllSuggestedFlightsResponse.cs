using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationSuggestion.Application.Dtos
{
    public class GetAllSuggestedFlightsResponse
    {
        public List<SuggestedFlightDto>? FlightsForGoing { get; set; }
        public List<SuggestedFlightDto>? FlightsForReturning { get; set; }

        public GetAllSuggestedFlightsResponse(List<SuggestedFlightDto>? flightsForGoing, List<SuggestedFlightDto>? flightsForReturning)
        {
            FlightsForGoing = flightsForGoing;
            FlightsForReturning = flightsForReturning;
        }
    }
}

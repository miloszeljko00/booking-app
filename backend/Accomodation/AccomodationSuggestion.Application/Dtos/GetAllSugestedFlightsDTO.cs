using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationSuggestion.Application.Dtos
{
    public class GetAllSugestedFlightsDTO
    {
        public string PlaceOfDeparture { get; set; }
        public DateTime DepartureDate { get; set; }
        public string PlaceOfArrival { get; set; }
        public DateTime ArrivalDate { get; set; }
    }
}

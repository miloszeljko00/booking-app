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
        public DateTime FirstDayDate { get; set; }
        public string PlaceOfArrival { get; set; }
        public DateTime LastDayDate { get; set; }
    }
}

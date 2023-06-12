using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationSuggestion.Application.Dtos
{
    public class BookFlightDto
    {
        public string UserId { get; set; }
        public string FlightId { get; set; }
        public int NumberOfTickets { get; set; }

    }
}

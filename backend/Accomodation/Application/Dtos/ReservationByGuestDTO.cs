﻿using AccomodationSuggestionDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accomodation.Application.Dtos
{
    public class ReservationByGuestDTO
    {
        public string Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int TotalPrice { get; set; }
        public string AccommodationName { get; set; }
        public string AccommodationId { get; set; }
        public string IsCanceled { get; set; }
        public int GuestNumber { get; set; }
    }
}

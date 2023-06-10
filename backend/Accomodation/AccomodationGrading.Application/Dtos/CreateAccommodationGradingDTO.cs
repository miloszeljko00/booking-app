using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationGradingApplication.Dtos
{
    public class CreateAccommodationGradingDTO
    {
        public string AccommodationName { get; set; }
        public string GuestEmail { get; set; }
        public string HostEmail { get; set; }
        public int Grade { get; set; }
    }
}

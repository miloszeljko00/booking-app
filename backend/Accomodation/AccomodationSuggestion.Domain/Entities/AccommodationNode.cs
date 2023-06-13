using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationSuggestion.Domain.Entities
{
    public class AccommodationNode
    {
        public string HostEmail { get; set; }
        public string AccommodationId { get; set; }
        public string AccommodationName { get; set; }

        public AccommodationNode(string hostEmail, string accommodationId, string accommodationName)
        {
            HostEmail = hostEmail;
            AccommodationId = accommodationId;
            AccommodationName = accommodationName;
        }
    }
}

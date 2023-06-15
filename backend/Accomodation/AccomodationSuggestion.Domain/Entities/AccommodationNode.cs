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
        
        public string AccommodationName { get; set; }

        public float AverageGrade { get; set; }

        public AccommodationNode(string hostEmail, string accommodationName)
        {
            AverageGrade = 0;
            HostEmail = hostEmail;
            AccommodationName = accommodationName;
        }
    }
}

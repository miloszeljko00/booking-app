using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accomodation.Application.Dtos
{
    public class PriceDTO
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public double Value { get; set; }
        public Guid AccommodationId { get; set; }

    }
}

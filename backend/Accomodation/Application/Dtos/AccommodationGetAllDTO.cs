using AccomodationDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accomodation.Application.Dtos
{
    public class AccommodationGetAllDTO
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public string Benefits { get; set; }
        public string PriceCalculation { get; set; }
        public string Id { get; set; }
        public double Price { get; set; }

    }
}

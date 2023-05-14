using Accomodation.Domain.Primitives.Enums;
using AccomodationSuggestionDomain.Entities;
using AccomodationSuggestionDomain.Primitives.Enums;
using AccomodationSuggestionDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//TODO: KREIRATI PRAVI DTO GDE CE SVAKI OD PROPERTIJA KOJI SU KLASE TAKODJE IMATI SVOJ DTO
namespace AccomodationApplication.Dtos
{
    public class AccommodationDTO
    {
        public string Name { get; set; }
        public Address Address { get; set; }
        public PriceCalculation PriceCalculation { get; set; }
        public List<Price> PricePerGuest { get; set; }
        public List<Benefit> Benefits { get; set; }
        public List<Picture> Pictures { get; set; }
        public Capacity Capacity { get; set; }
        public bool ReserveAutomatically { get; set; }
        public string HostEmail { get; set; }
    }
}

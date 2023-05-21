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
    public class AccommodationMainDTO
    {
        public string Name { get; set; }
        public string HostEmail { get; set; }
    }
}

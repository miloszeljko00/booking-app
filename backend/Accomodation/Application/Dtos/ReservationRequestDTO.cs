using Accomodation.Domain.Primitives.Enums;
using AccomodationDomain.Entities;
using AccomodationDomain.Primitives.Enums;
using AccomodationDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//TODO: KREIRATI PRAVI DTO GDE CE SVAKI OD PROPERTIJA KOJI SU KLASE TAKODJE IMATI SVOJ DTO
namespace AccomodationApplication.Dtos
{
    public class ReservationRequestDTO
    {
        public Guid AccommodationId { get; set; }
        public string GuestEmail { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int numberOfGuests { get; set; }

    }
}

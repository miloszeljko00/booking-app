using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accomodation.Application.Dtos
{
    public class ReservationCancellationDTO
    {
        public Guid AccommodationId { get; set; }
        public Guid ReservationId { get; set; }
    }
}

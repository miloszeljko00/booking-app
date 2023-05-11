using Accomodation.Domain.Primitives.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accomodation.Application.Dtos
{
    public class ReservationRequestByAdminDTO
    {
        public string Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Status { get; set; }
        public string AccommodationName { get; set; }
        public string AccommodationId { get; set; }
        public int GuestNumber { get; set; }
        public int CancellationNumber { get; set; }
        public string GuestEmail { get; set; }
    }
}

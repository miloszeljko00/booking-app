using Accomodation.Application.Dtos;
using AccomodationApplication.Abstractions.Messaging;
using AccomodationDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationApplication.Accommodation.Queries
{
    public sealed record GetAllReservationsByGuestQuery(string guestEmail) : IQuery<ICollection<ReservationByGuestDTO>>
    {
    }
}

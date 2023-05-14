using Accomodation.Application.Dtos;
using AccomodationApplication.Abstractions.Messaging;
using AccomodationSuggestionDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationApplication.Accommodation.Queries
{
    public sealed record GetAllRequestsByGuestQuery(string guestEmail) : IQuery<ICollection<ReservationRequestByGuestDTO>>
    {
    }
}

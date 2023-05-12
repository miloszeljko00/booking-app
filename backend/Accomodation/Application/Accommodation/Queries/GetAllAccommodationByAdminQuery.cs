using Accomodation.Application.Dtos;
using AccomodationApplication.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accomodation.Application.Accommodation.Queries
{
    public sealed record GetAllAccommodationByAdminQuery(string adminEmail) : IQuery<ICollection<AccommodationGetAllDTO>>
    {
    }
}

using Accomodation.Application.Dtos;
using AccomodationApplication.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Accomodation.Application.Accommodation.Queries
{
    public sealed record SearchAccommodationQuery(string address, int numberOfGuests, string startDate, string endDate) : IQuery<ICollection<AccommodationGetAllDTO>>
    {
    }
}

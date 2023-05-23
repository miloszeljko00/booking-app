using AccomodationGradingApplication.Abstractions.Messaging;
using AccomodationGradingApplication.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationGradingApplication.Grading.Queries
{
    public sealed record GetAccommodationGradingQuery() : IQuery<List<AccommodationGradingDTO>>
    {
    }
}

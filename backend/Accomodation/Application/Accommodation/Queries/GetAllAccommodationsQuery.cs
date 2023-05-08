using AccomodationApplication.Abstractions.Messaging;
using AccomodationDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationApplication.Accommodation.Queries
{
    public class GetAllAccommodationsQuery : IQuery<IReadOnlyCollection<AccomodationDomain.Entities.Accommodation>>
    {
    }
}

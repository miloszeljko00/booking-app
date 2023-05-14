using AccomodationApplication.Abstractions.Messaging;
using AccomodationSuggestionDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationApplication.Accommodation.Queries
{
    public class GetAllAccommodationsQuery : IQuery<ICollection<AccomodationSuggestionDomain.Entities.Accommodation>>
    {
    }
}

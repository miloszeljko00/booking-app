using AccomodationGradingApplication.Abstractions.Messaging;
using AccomodationGradingApplication.Dtos;
using AccomodationGradingDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationGradingApplication.Grading.Commands
{
    public sealed record UpdateAccommodationGradingCommand(
        UpdateAccommodationGradingDTO updateAccommodationGradingDTO
        ) : ICommand<AccommodationGrading>
    {
    }
}

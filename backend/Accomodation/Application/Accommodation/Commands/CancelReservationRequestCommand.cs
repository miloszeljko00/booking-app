using Accomodation.Application.Dtos;
using AccomodationApplication.Abstractions.Messaging;
using AccomodationApplication.Dtos;
using AccomodationSuggestionDomain.Entities;
using AccomodationSuggestionDomain.Primitives.Enums;
using AccomodationSuggestionDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationApplication.Accommodation.Commands
{
    public sealed record CancelReservationRequestCommand(
        ReservationCancellationDTO reservationRequestCancellationDTO
        ) : ICommand<AccomodationSuggestionDomain.Entities.Accommodation>
    {
    }
}

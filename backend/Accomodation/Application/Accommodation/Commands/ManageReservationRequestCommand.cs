using Accomodation.Application.Dtos;
using AccomodationApplication.Abstractions.Messaging;
using AccomodationApplication.Dtos;
using AccomodationDomain.Entities;
using AccomodationDomain.Primitives.Enums;
using AccomodationDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationApplication.Accommodation.Commands
{
    public sealed record ManageReservationRequestCommand(
        ReservationManagementDTO reservationRequestManagementDTO
        ) : ICommand<AccomodationDomain.Entities.Accommodation>
    {
    }
}

using Application.Abstractions.Messaging;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AccommodationOfferFolder.Commands
{
    public sealed record CreateAccommodationOfferCommand(
        string hotelName,
        DateTime start,
        DateTime end) : ICommand<AccommodationOffer>
    {
    }
}

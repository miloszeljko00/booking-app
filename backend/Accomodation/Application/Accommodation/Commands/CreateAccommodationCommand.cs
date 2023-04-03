using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Entities;
using Domain.Primitives.Enums;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Accommodation.Commands
{
    public sealed record CreateAccommodationCommand(
        AccommodationDTO AccommodationDto
        ) : ICommand<Domain.Entities.Accommodation>
    {
    }
}

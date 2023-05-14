using Accomodation.Application.Dtos;
using AccomodationApplication.Abstractions.Messaging;
using AccomodationSuggestionDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accomodation.Application.Accommodation.Commands
{
    public sealed record AddPriceCommand(PriceDTO priceDTO) : ICommand<AccomodationSuggestionDomain.Entities.Accommodation>
    {
    }
    
}

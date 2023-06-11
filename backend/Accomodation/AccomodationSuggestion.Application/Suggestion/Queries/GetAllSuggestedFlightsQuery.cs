using AccomodationApplication.Abstractions.Messaging;
using AccomodationSuggestion.Application.Dtos;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationSuggestion.Application.Suggestion.Queries
{
    public sealed record GetAllSuggestedFlightsQuery(GetAllSugestedFlightsDTO queryDto) : IQuery<GetAllSuggestedFlightsResponse>
    {

    }
}

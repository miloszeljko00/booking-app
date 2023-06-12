using AccomodationApplication.Abstractions.Messaging;
using AccomodationSuggestion.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationSuggestion.Application.Suggestion.Queries
{
    public sealed record GetAllUserNodesQuery(): IQuery<UserNodeDTO>
    {
    }
    
}

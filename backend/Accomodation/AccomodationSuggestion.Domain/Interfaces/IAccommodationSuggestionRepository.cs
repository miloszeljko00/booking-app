using AccomodationSuggestion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationSuggestion.Domain.Interfaces
{
    public interface IAccommodationSuggestionRepository
    {
        public Task<List<UserNode>> getAllUserNodesAsync();
    }
}

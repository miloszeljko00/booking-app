using AccomodationApplication.Abstractions.Messaging;
using AccomodationSuggestion.Application.Dtos;
using AccomodationSuggestion.Domain.Entities;
using AccomodationSuggestion.Domain.Interfaces;

namespace AccomodationSuggestion.Application.Suggestion.Queries
{
    public sealed class GetAllUserNodesQueryHandler : IQueryHandler<GetAllUserNodesQuery, UserNode>
    {
        private readonly IAccommodationSuggestionRepository _repository;
        public GetAllUserNodesQueryHandler(IAccommodationSuggestionRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserNode> Handle(GetAllUserNodesQuery request, CancellationToken cancellationToken)
        {
            List<UserNode> users = _repository.getAllUserNodesAsync().Result;
            return users.First();
        }
    }
   
}

using AccomodationApplication.Abstractions.Messaging;
using AccomodationSuggestion.Application.Dtos;
using AccomodationSuggestion.Domain.Interfaces;

namespace AccomodationSuggestion.Application.Suggestion.Queries
{
    public sealed class GetAllUserNodesQueryHandler : IQueryHandler<GetAllUserNodesQuery, UserNodeDTO>
    {
        private readonly IAccommodationSuggestionRepository _repository;
        public Task<UserNodeDTO> Handle(GetAllUserNodesQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
    /*public sealed class GetAllAccommodationsQueryHandler : IQueryHandler<GetAllAccommodationsQuery, ICollection<AccomodationSuggestionDomain.Entities.Accommodation>>
    {
        private readonly IAccommodationRepository _repository;
        public GetAllAccommodationsQueryHandler(IAccommodationRepository repository)
        {
            _repository = repository;
        }

        public async Task<ICollection<AccomodationSuggestionDomain.Entities.Accommodation>> Handle(GetAllAccommodationsQuery request, CancellationToken cancellationToken)
        {

            return await _repository.GetAllAsync();
        }
    }*/
}

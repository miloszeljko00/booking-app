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
            //var userNode = _repository.createUserAsync("proba@mail.com");
            //var accNode = _repository.createAccommodationNode(new AccommodationNode("aaa@mail", "id", "accname"));
            //var iscreated = _repository.createGradeRelationship(4, "15f", "proba@mail.com");
            //var accNodes = _repository.getAccommodationLikedBySimilarUsers("ime@ime.com");
            var rez1 = _repository.getNumberOfRecentBadGrades("Acc1");
            var rez2 = _repository.getAverageGrade("Acc1");
            return users.First();
        }
    }
   
}

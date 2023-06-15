using AccomodationApplication.Abstractions.Messaging;
using AccomodationSuggestion.Domain.Entities;
using AccomodationSuggestion.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationSuggestion.Application.Suggestion.Queries
{
    public class GetRecommendedAccommodationQueryHandler : IQueryHandler<GetRecommendedAccommodationQuery, List<AccommodationNode>>
    {
        private readonly IAccommodationSuggestionRepository _repository;

        public GetRecommendedAccommodationQueryHandler(IAccommodationSuggestionRepository repository)
        {
            _repository = repository;
        }

        public Task<List<AccommodationNode>> Handle(GetRecommendedAccommodationQuery request, CancellationToken cancellationToken)
        {
            var accLikedBySimilarUsers = _repository.getAccommodationLikedBySimilarUsers(request.guestEmail).Result;
            List<AccommodationNode> accList = new List<AccommodationNode>();
            foreach(var acc in accLikedBySimilarUsers)
            {
                int numberOfBadGrades = _repository.getNumberOfRecentBadGrades(acc.AccommodationName).Result;
                if (numberOfBadGrades > 5)
                    continue;
                acc.AverageGrade = _repository.getAverageGrade(acc.AccommodationName).Result;
                accList.Add(acc);
            }
            var sortedList =accList.OrderByDescending(acc => acc.AverageGrade).ToList();
            return Task.FromResult(sortedList.Take(5).ToList());
        }
    }
}

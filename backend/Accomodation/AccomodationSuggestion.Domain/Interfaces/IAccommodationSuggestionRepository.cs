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
        public Task<UserNode> createUserAsync(string email);
        public Task<AccommodationNode> createAccommodationNode(AccommodationNode accommodationNode);
        public Task<bool> createGradeRelationship(int grade, string accommodationName, string email, string date);
        public Task<List<AccommodationNode>> getAccommodationLikedBySimilarUsers(string email);
        public Task<int> getNumberOfRecentBadGrades(string accommodationName);
        public Task<float> getAverageGrade(string accommodationName);
    }
}

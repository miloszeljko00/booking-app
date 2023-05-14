using AccomodationApplication.Abstractions.Messaging;
using AccomodationSuggestionDomain.Entities;
using AccomodationSuggestionDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationApplication.Accommodation.Queries
{
    public sealed class GetAllAccommodationsQueryHandler : IQueryHandler<GetAllAccommodationsQuery, ICollection<AccomodationSuggestionDomain.Entities.Accommodation>>
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
    }
}

using AccomodationApplication.Abstractions.Messaging;
using AccomodationDomain.Entities;
using AccomodationDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationApplication.Accommodation.Queries
{
    public sealed class GetAllAccommodationsQueryHandler : IQueryHandler<GetAllAccommodationsQuery, IReadOnlyCollection<AccomodationDomain.Entities.Accommodation>>
    {
        private readonly IAccommodationRepository _repository;
        public GetAllAccommodationsQueryHandler(IAccommodationRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyCollection<AccomodationDomain.Entities.Accommodation>> Handle(GetAllAccommodationsQuery request, CancellationToken cancellationToken)
        {

            return await _repository.GetAllAsync();
        }
    }
}

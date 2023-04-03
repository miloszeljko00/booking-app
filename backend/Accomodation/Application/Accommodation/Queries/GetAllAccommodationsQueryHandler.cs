using Application.Abstractions.Messaging;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Accommodation.Queries
{
    public sealed class GetAllAccommodationsQueryHandler : IQueryHandler<GetAllAccommodationsQuery, IReadOnlyCollection<Domain.Entities.Accommodation>>
    {
        private readonly IAccommodationRepository _repository;
        public GetAllAccommodationsQueryHandler(IAccommodationRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyCollection<Domain.Entities.Accommodation>> Handle(GetAllAccommodationsQuery request, CancellationToken cancellationToken)
        {

            return await _repository.GetAllAsync();
        }
    }
}

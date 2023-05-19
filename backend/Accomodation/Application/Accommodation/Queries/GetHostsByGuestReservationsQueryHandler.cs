using Accomodation.Application.Dtos;
using Accomodation.Domain.Primitives.Enums;
using AccomodationApplication.Abstractions.Messaging;
using AccomodationSuggestionDomain.Entities;
using AccomodationSuggestionDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationApplication.Accommodation.Queries
{
    public sealed class GetHostsByGuestReservationsQueryHandler : IQueryHandler<GetHostsByGuestReservationsQuery, ICollection<string>>
    {
        private readonly IAccommodationRepository _repository;
        public GetHostsByGuestReservationsQueryHandler(IAccommodationRepository repository)
        {
            _repository = repository;
        }

        public async Task<ICollection<string>> Handle(GetHostsByGuestReservationsQuery request, CancellationToken cancellationToken)
        {
            var accs = await _repository.GetAllAsync();
            List<AccomodationSuggestionDomain.Entities.Accommodation> accommodations = accs.ToList();
            List<string> hosts = new List<string>();
            foreach(var acc in accommodations)
            {
                if (acc.DoesGuestHasReservation(request.guestEmail))
                {
                    if (!hosts.Contains(acc.HostEmail.EmailAddress))
                    {
                        hosts.Add(acc.HostEmail.EmailAddress);
                    }
                }
            }
            return hosts;
        }
    }
}

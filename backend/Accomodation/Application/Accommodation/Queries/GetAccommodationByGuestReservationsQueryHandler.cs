using Accomodation.Application.Dtos;
using Accomodation.Domain.Primitives.Enums;
using AccomodationApplication.Abstractions.Messaging;
using AccomodationApplication.Dtos;
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
    public sealed class GetAccommodationByGuestReservationsQueryHandler : IQueryHandler<GetAccommodationByGuestReservationsQuery, ICollection<AccommodationMainDTO>>
    {
        private readonly IAccommodationRepository _repository;
        public GetAccommodationByGuestReservationsQueryHandler(IAccommodationRepository repository)
        {
            _repository = repository;
        }

        public async Task<ICollection<AccommodationMainDTO>> Handle(GetAccommodationByGuestReservationsQuery request, CancellationToken cancellationToken)
        {
            var accs = await _repository.GetAllAsync();
            List<AccomodationSuggestionDomain.Entities.Accommodation> accommodations = accs.ToList();
            List<AccommodationMainDTO> accMainDTOs = new List<AccommodationMainDTO>();
            foreach(var acc in accommodations)
            {
                if (acc.DoesGuestHasReservation(request.guestEmail))
                {
                    AccommodationMainDTO accMainDTO = new AccommodationMainDTO { Name = acc.Name, HostEmail = acc.HostEmail.EmailAddress };
                    if (!accMainDTOs.Contains(accMainDTO))
                    {
                        accMainDTOs.Add(accMainDTO);
                    }
                }
            }
            return accMainDTOs;
        }
    }
}

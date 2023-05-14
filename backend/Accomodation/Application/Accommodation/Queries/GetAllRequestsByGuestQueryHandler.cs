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
    public sealed class GetAllRequestsByGuestQueryHandler : IQueryHandler<GetAllRequestsByGuestQuery, ICollection<ReservationRequestByGuestDTO>>
    {
        private readonly IAccommodationRepository _repository;
        public GetAllRequestsByGuestQueryHandler(IAccommodationRepository repository)
        {
            _repository = repository;
        }

        public async Task<ICollection<ReservationRequestByGuestDTO>> Handle(GetAllRequestsByGuestQuery request, CancellationToken cancellationToken)
        {
            var accs = await _repository.GetAllAsync();
            List<AccomodationSuggestionDomain.Entities.Accommodation> accommodations = accs.ToList();
            ICollection<ReservationRequestByGuestDTO> response = new Collection<ReservationRequestByGuestDTO>();
            foreach (AccomodationSuggestionDomain.Entities.Accommodation acc in accommodations)
            {
                List<ReservationRequest> requests = acc.ReservationRequests;
                foreach (ReservationRequest req in requests)
                {
                    if (req.GuestEmail.EmailAddress.Equals(request.guestEmail) && !req.Status.Equals(ReservationRequestStatus.CANCELED))
                        response.Add(new ReservationRequestByGuestDTO
                        {
                            Id = req.Id.ToString(),
                            Start = req.ReservationDate.Start,
                            End = req.ReservationDate.End,
                            GuestNumber = req.GuestNumber,
                            Status = req.Status.ToString(),
                            AccommodationName = acc.Name,
                            AccommodationId = acc.Id.ToString()
                        });
                }
            }
            return response;
        }
    }
}

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
    public sealed class GetAllRequestsByAdminQueryHandler : IQueryHandler<GetAllRequestsByAdminQuery, ICollection<ReservationRequestByAdminDTO>>
    {
        private readonly IAccommodationRepository _repository;
        public GetAllRequestsByAdminQueryHandler(IAccommodationRepository repository)
        {
            _repository = repository;
        }

        public async Task<ICollection<ReservationRequestByAdminDTO>> Handle(GetAllRequestsByAdminQuery request, CancellationToken cancellationToken)
        {
            var accs = await _repository.GetAllAsync();
            List<AccomodationSuggestionDomain.Entities.Accommodation> accommodationList = accs.ToList();
            List<AccomodationSuggestionDomain.Entities.Accommodation> accommodations = new List<AccomodationSuggestionDomain.Entities.Accommodation>();
            foreach(AccomodationSuggestionDomain.Entities.Accommodation acc in accommodationList)
            {
                if (acc.HostEmail.EmailAddress.Equals(request.adminEmail))
                {
                    accommodations.Add(acc);
                }
            }

            ICollection<ReservationRequestByAdminDTO> response = new Collection<ReservationRequestByAdminDTO>();
            foreach (AccomodationSuggestionDomain.Entities.Accommodation acc in accommodations)
            {
                List<ReservationRequest> requests = acc.ReservationRequests;
                foreach (ReservationRequest req in requests)
                {
                    if (!req.Status.Equals(ReservationRequestStatus.CANCELED))
                        response.Add(new ReservationRequestByAdminDTO
                        {
                            Id = req.Id.ToString(),
                            Start = req.ReservationDate.Start,
                            End = req.ReservationDate.End,
                            GuestNumber = req.GuestNumber,
                            Status = req.Status.ToString(),
                            AccommodationName = acc.Name,
                            AccommodationId = acc.Id.ToString(),
                            CancellationNumber = 0,
                            GuestEmail = req.GuestEmail.EmailAddress
                        });
                }
            }

            int totalCancellationNumber = 0;
            foreach (ReservationRequestByAdminDTO req in response)
            {
                foreach (AccomodationSuggestionDomain.Entities.Accommodation acc in accs)
                {
                    totalCancellationNumber += acc.GetCancellationNumber(req.GuestEmail);
                }
                req.CancellationNumber = totalCancellationNumber;
                totalCancellationNumber = 0;
            }

            return response;
        }
    }
}

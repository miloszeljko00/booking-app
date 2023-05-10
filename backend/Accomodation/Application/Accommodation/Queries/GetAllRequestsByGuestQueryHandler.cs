﻿using Accomodation.Application.Dtos;
using AccomodationApplication.Abstractions.Messaging;
using AccomodationDomain.Entities;
using AccomodationDomain.Interfaces;
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
            List<AccomodationDomain.Entities.Accommodation> accommodations = accs.ToList();
            ICollection<ReservationRequestByGuestDTO> response = new Collection<ReservationRequestByGuestDTO>();
            foreach (AccomodationDomain.Entities.Accommodation acc in accommodations)
            {
                List<ReservationRequest> requests = acc.ReservationRequests;
                foreach (ReservationRequest req in requests)
                {
                    if (req.GuestEmail.EmailAddress.Equals(request.guestEmail))
                        response.Add(new ReservationRequestByGuestDTO
                        {
                            Id = req.Id.ToString(),
                            Start = req.ReservationDate.Start,
                            End = req.ReservationDate.End,
                            GuestNumber = req.GuestNumber,
                            Status = req.Status.ToString()
                        });
                }
            }
            return response;
        }
    }
}

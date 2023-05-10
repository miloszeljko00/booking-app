using Accomodation.Application.Dtos;
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
    public sealed class GetAllReservationsByGuestQueryHandler : IQueryHandler<GetAllReservationsByGuestQuery, ICollection<ReservationByGuestDTO>>
    {
        private readonly IAccommodationRepository _repository;
        public GetAllReservationsByGuestQueryHandler(IAccommodationRepository repository)
        {
            _repository = repository;
        }

        public async Task<ICollection<ReservationByGuestDTO>> Handle(GetAllReservationsByGuestQuery request, CancellationToken cancellationToken)
        {
            var accs = await _repository.GetAllAsync();
            List<AccomodationDomain.Entities.Accommodation> accommodations = accs.ToList();
            ICollection<ReservationByGuestDTO> response = new Collection<ReservationByGuestDTO>();
            foreach (AccomodationDomain.Entities.Accommodation acc in accommodations)
            {
                List<Reservation> reservations = acc.Reservations;
                foreach (Reservation res in reservations)
                {
                    if (res.GuestEmail.EmailAddress.Equals(request.guestEmail) && !res.IsCanceled)
                        response.Add(new ReservationByGuestDTO
                        {
                            Id = res.Id.ToString(),
                            Start = res.ReservationDate.Start,
                            End = res.ReservationDate.End,
                            GuestNumber = res.GuestNumber,
                            TotalPrice = res.TotalPrice,
                            IsCanceled = res.IsCanceled == true ? "CANCELED" : "ACTIVE",
                            AccommodationName = acc.Name
                        });
                }
            }
            return response;
        }
    }
}

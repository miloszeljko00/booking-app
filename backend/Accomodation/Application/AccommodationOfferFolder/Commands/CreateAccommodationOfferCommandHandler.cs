using Application.Abstractions.Messaging;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AccommodationOfferFolder.Commands
{
    public sealed class CreateAccommodationOfferCommandHandler : ICommandHandler<CreateAccommodationOfferCommand, AccommodationOffer>
    {
        private readonly IAccommodationOfferRepository _accommodationOfferRepository;
        public CreateAccommodationOfferCommandHandler(IAccommodationOfferRepository accommodationOfferRepository)
        {
            _accommodationOfferRepository = accommodationOfferRepository;
        }

        Task<AccommodationOffer> IRequestHandler<CreateAccommodationOfferCommand, AccommodationOffer>.Handle(CreateAccommodationOfferCommand request, CancellationToken cancellationToken)
        {
            var accommodationOffer = AccommodationOffer.Create(Guid.NewGuid(), request.hotelName, request.start, request.end);
            return _accommodationOfferRepository.Create(accommodationOffer);
        }
    }
}

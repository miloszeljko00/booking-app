using AccomodationApplication.Abstractions.Messaging;
using AccomodationDomain.Entities;
using AccomodationDomain.Interfaces;
using AccomodationDomain.Primitives.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccomodationApplication.Accommodation.Commands
{
    public sealed class CreateAccommodationCommandHandler : ICommandHandler<CreateAccommodationCommand, AccomodationDomain.Entities.Accommodation>
    {
        private readonly IAccommodationRepository _repository;
        public AccommodationBuilder AccommodationBuilder { get; set; } = new AccommodationBuilder();
        public CreateAccommodationCommandHandler(IAccommodationRepository repository)
        {
            _repository = repository;
        }

        public Task<AccomodationDomain.Entities.Accommodation> Handle(CreateAccommodationCommand request, CancellationToken cancellationToken)
        {
            var accommodationBuilder = AccommodationBuilder.withAddress(request.AccommodationDto.Address.Country,
                request.AccommodationDto.Address.City, request.AccommodationDto.Address.Street, request.AccommodationDto.Address.Number)
                                .withPriceCalculation(request.AccommodationDto.PriceCalculation)
                                .withPricePerGuest(request.AccommodationDto.PricePerGuest)
                                .withName(request.AccommodationDto.Name)
                                .withCapacity(request.AccommodationDto.Capacity.Max, request.AccommodationDto.Capacity.Min)
                                .withAutomaticallyReservation(request.AccommodationDto.ReserveAutomatically);
            foreach (var picture in request.AccommodationDto.Pictures)
            {
                accommodationBuilder.withPicture(picture.FileName);
            }
            //var benefits = request.AccommodationDto.Benefits
            //    .Select(b => (Benefit)Enum.Parse(typeof(Benefit), b))
            //    .ToList();
            var benefits = new List<Benefit>();
            foreach (var benefit in request.AccommodationDto.Benefits)
            {
                benefits.Add(benefit);
            }
            accommodationBuilder.withBenefits(benefits);
            var accommodation = accommodationBuilder.build();
            return _repository.Create(accommodation);
        }
    }
}

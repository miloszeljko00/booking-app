using Application.Abstractions.Messaging;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Primitives.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Accommodation.Commands
{
    public sealed class CreateAccommodationCommandHandler : ICommandHandler<CreateAccommodationCommand, Domain.Entities.Accommodation>
    {
        private readonly IAccommodationRepository _repository;
        public AccommodationBuilder AccommodationBuilder { get; set; } = new AccommodationBuilder();
        public CreateAccommodationCommandHandler(IAccommodationRepository repository)
        {
            _repository = repository;
        }

        public Task<Domain.Entities.Accommodation> Handle(CreateAccommodationCommand request, CancellationToken cancellationToken)
        {
            var accommodationBuilder = AccommodationBuilder.withAddress(request.AccommodationDto.Address.Country,
                request.AccommodationDto.Address.City, request.AccommodationDto.Address.Street, request.AccommodationDto.Address.Number)
                                .withPricePerGuest(999.99)
                                .withName(request.AccommodationDto.Name)
                                .withPicture("velja_slika.jpg", "velja opis slike")
                                .withCapacity(request.AccommodationDto.Capacity.Max, request.AccommodationDto.Capacity.Min)
                                .withBenefits(new List<Benefit>
                                {
                                    Benefit.WI_FI,
                                    Benefit.KITCHEN,
                                    Benefit.AIR_CONDITIONING
                                })
                                .withAutomaticallyReservation(request.AccommodationDto.ReserveAutomatically);
            foreach (var picture in request.AccommodationDto.Pictures)
            {
                accommodationBuilder.withPicture(picture.FileName, picture.Description);
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
